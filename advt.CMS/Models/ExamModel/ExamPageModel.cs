using advt.Entity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace advt.CMS.Models
{
    public class ExamPageModel
    {
        public List<ExamScoreInfo> Listexam { get; set; }
        public ExamRule Current { get; set; }
        public ExamView examList { get; set; }
        public List<ExamView> ListBankView { get; set; }
        public ExamUserInfo VExamUserInfo { get; set; }
        public int nowItem { get; set; }
        public int total { get; set; }
        public string PreNext { get; set; }
         public ExamUserInfo TestD { get; set; }
        public string ID { get; set; }
        public ExamScore VExamScore { get; set; }
        public string IsExam { get; set; }//是否可考
        public string IsTest { get; set; }
        public string RuleName { get; set; }
        public string ExamFailResult { get; set; }
        public List<ExamUserDetailInfo> ListExamUserDetailInfo { get; set; }
        public string Remark { get; set; }
        public ExamPageModel() : base()
        {
            examList = new ExamView();
            ListBankView = new List<ExamView>();
            TestD = new ExamUserInfo();
            VExamScore = new ExamScore();
            if (ListBankView.Count() >0)
            {
                var ss = Data.ExamScore.Get_All_ExamScore().Max(x => x.ExamID);
                VExamScore = Data.ExamScore.Get_ExamScore(ss);
            }
            VExamUserInfo = new ExamUserInfo();
            VExamUserInfo.LExamViews = new List<ExamView>();
            ListExamUserDetailInfo = new List<ExamUserDetailInfo>();
            Listexam = new List<ExamScoreInfo>();
        }
        public string GetExamBankNum(string RuleName, string usercode)
        {
            var Result = "";
            //获取规则
            var Rule = Data.ExamRule.Get_ExamRule(new { RuleName });
            //规则里面题目类型和数量List
            if (Rule != null)
            {
                var topicnum = Data.ExamRuleTopicType.Get_All_ExamRuleTopicType_RuleId(Rule.ID);
                foreach (var item in topicnum)
                {
                    var banks = Data.ExamBank.Get_All_ExamBank_ExamType_Rule(item.TopicType, item.TopicMajor, item.TopicLevel, Rule.SubjectName);
                    int TopicNum = Convert.ToInt32(item.TopicNum);
                    var bank = banks.OrderBy(y => Guid.NewGuid()).Take(TopicNum);
                    if (TopicNum != bank.Count())
                    {
                        var c = "";
                        if (item.TopicType == "0")
                        {
                            c = "单选";
                        }
                        else if (item.TopicType == "1")
                        {
                            c = "多选";
                        }
                        else if (item.TopicType == "2")
                        {
                            c = "问答";
                        }
                        Result += item.TopicMajor + item.TopicLevel + Rule.SubjectName + c + " 在题库中数量不够,需要" + TopicNum + "道题目,题库中只有" + bank.Count() + "道题目";
                    }
                }
               
                var ListPract = new List<PracticeInfo>();
                if (!string.IsNullOrEmpty(Rule.SubjectName))
                {
                    ListPract = Data.PracticeInfo.Get_All_PracticeInfo(new { TypeName = Rule.TypeName, SubjectName = Rule.SubjectName, UserCode = usercode });
                }
                else
                {
                    ListPract = Data.PracticeInfo.Get_All_PracticeInfo(new { TypeName = Rule.TypeName, UserCode = usercode });
                }
                var Pract = ListPract.OrderByDescending(x => x.CreateDate).FirstOrDefault();
                if (Pract == null)
                {
                    Result += "未找到对应实践分数,不可报名。考试规则要求实践分数" + Rule.PassPracticeScore;
                }
                else if (Pract.PracticeScore < Rule.PassPracticeScore)
                {
                    Result += "实践分数未达标,不可报名。考试规则要求实践分数" + Rule.PassPracticeScore + "目前实践分数" + Pract.PracticeScore;
                }
            }

            return Result;
        }
        public void GetListExam(string data,string RuleName,string username)
        {
            try
            {
                var ListBanks = new List<ExamBankView>();
                var Rule = Data.ExamRule.Get_ExamRule(new {RuleName });
                var usersheet = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { EamilUsername = username });
                if (Rule != null)
                {
                    VExamUserInfo.ExamType = Rule.TypeName;//考试名称
                    VExamUserInfo.TotalScore = Rule.TotalScore;//总分
                    VExamUserInfo.ExamSubject = Rule.SubjectName;//考试科目
                    VExamUserInfo.IsQuestion = Rule.IsQuestion;
                    if (usersheet != null)
                    {
                        VExamUserInfo.UserName = usersheet.UserCode;//工号
                        VExamUserInfo.UserTextName = usersheet.UserName;
                    }
                    
                    if (data == "formal")
                    {
                        VExamUserInfo.IsTest = false;//是否模拟
                    }
                    else if(data=="test")
                    {
                        VExamUserInfo.IsTest = true;//是否模拟
                    }
                    VExamUserInfo.IsRead = Rule.IsRead;//是否审批
                    VExamUserInfo.TotalTime = Rule.TotalTime;
                    VExamUserInfo.StartDesc = Rule.StartDeac;
                    VExamUserInfo.EndDesc = Rule.EndDesc;
                    VExamUserInfo.PassScore = Rule.PassScore;
                    var topicnum = Data.ExamRuleTopicType.Get_All_ExamRuleTopicType_RuleId(Rule.ID);
                    foreach (var item in topicnum)
                    {
                        var banks = Data.ExamBank.Get_All_ExamBank_ExamType_Rule(item.TopicType, item.TopicMajor, item.TopicLevel, Rule.SubjectName);
                        int TopicNum = Convert.ToInt32(item.TopicNum);
                        var bank = banks.OrderBy(y => Guid.NewGuid()).Take(TopicNum);
                        foreach (var items in bank)
                        {
                            var right = items.RightKey.Split(',').OrderBy(x => x).ToArray();
                            ListBanks.Add(new ExamBankView
                            {
                                ID = items.ID,
                                TopicType = items.TopicType,
                                TopicTitle = items.TopicTitle,
                                OptionA = items.OptionA,
                                OptionAPicNum = items.OptionAPicNum,
                                OptionB = items.OptionB,
                                OptionBPicNum = items.OptionBPicNum,
                                OptionC = items.OptionC,
                                OptionCPicNum = items.OptionCPicNum,
                                OptionD = items.OptionD,
                                OptionDPicNum = items.OptionDPicNum,
                                OptionE = items.OptionE,
                                OptionEPicNum = items.OptionEPicNum,
                                OptionF = items.OptionF,
                                OptionFPicNum = items.OptionFPicNum,
                                Score = Convert.ToInt32(item.TopicScore),
                                TotalScore = Convert.ToDecimal(item.TopicScore),
                                RightKey = right,
                                Remark=items.Remark,
                                TopicTitlePic= items.TopicTitlePicNum,
                                TopicLevel=items.TopicLevel,
                                TopicMajor = items.TopicMajor
                            });
                        }
                    }
                    ListBanks = ListBanks.OrderBy(y => Guid.NewGuid()).ToList();
                    total = ListBanks.Count();
                    nowItem = 1;
                    var index = 1;
                    //找到符合要求的题目
                    foreach (var item in ListBanks)
                    {
                        List<LAnsower> answ = new List<LAnsower>();
                        if (!string.IsNullOrEmpty(item.OptionA))
                        {
                            answ.Add(new LAnsower
                            {
                                ansower = item.OptionA,
                                ansowerpic = item.OptionAPicNum,
                                ansowerflag = "A",
                            });
                        }
                        if (!string.IsNullOrEmpty(item.OptionB))
                        {
                            answ.Add(new LAnsower
                            {
                                ansower = item.OptionB,
                                ansowerpic = item.OptionBPicNum,
                                ansowerflag = "B",
                            });
                        }
                        if (!string.IsNullOrEmpty(item.OptionC))
                        {
                            answ.Add(new LAnsower
                            {
                                ansower = item.OptionC,
                                ansowerpic = item.OptionCPicNum,
                                ansowerflag = "C",
                            });
                        }
                        if (!string.IsNullOrEmpty(item.OptionD))
                        {
                            answ.Add(new LAnsower
                            {
                                ansower = item.OptionD,
                                ansowerpic = item.OptionDPicNum,
                                ansowerflag = "D",
                            });
                        }
                        if (!string.IsNullOrEmpty(item.OptionE))
                        {
                            answ.Add(new LAnsower
                            {
                                ansower = item.OptionE,
                                ansowerpic = item.OptionEPicNum,
                                ansowerflag = "E",
                            });
                        }
                        var typename = "";
                        if (item.TopicType == "0")
                        {
                            typename = "单选";
                        }
                        else if(item.TopicType == "1")
                        {
                            typename = "多选";
                        }
                        else if (item.TopicType == "2")
                        {
                            typename = "问答";
                        }
                        ListBankView.Add(new ExamView
                        {
                            id = item.ID.ToString(),
                            type = item.TopicType,//题目类型
                            proName = item.TopicTitle,//题目标题
                            ansowerList = answ.OrderBy(y => Guid.NewGuid()).ToList(),//选项内容List
                            index = index,//下标
                            RightKey = item.RightKey,//正确答案
                            Remark = item.Remark,//答案解析
                            TopicScore= item.Score,//单题分数
                            ExamTypeName= typename,
                            TopicTitlePic=item.TopicTitlePic,
                            TopicLevel=item.TopicLevel,
                            TopicMajor=item.TopicMajor

                        });
                        index++;
                    }
                    if (ListBankView.Count() != 0)
                    {
                        examList = ListBankView.OrderBy(x => x.index).FirstOrDefault();
                    }
                    VExamUserInfo.LExamViews = new List<ExamView>();
                    VExamUserInfo.LExamViews.AddRange(ListBankView);
                }
               

            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public void GetExam()
        {
            foreach (var item in ListBankView)
            {
                if (item.index == nowItem)
                {
                    item.selectItem = examList.selectItem;
                    //item.ansower = examList.ansower;
                    item.LselectItem = examList.LselectItem;
                }
            }
            if (PreNext == "Pre")
            {
                nowItem = nowItem - 1;
            }
            if (PreNext == "Next")
            {
                nowItem = nowItem + 1;
            }
            examList = ListBankView.Where(x => x.index == nowItem).FirstOrDefault();
            VExamUserInfo.LExamViews = ListBankView;

        }
        public void InsertResult()
        {
            var c = this.ListBankView;
            foreach (var item in this.ListBankView)
            {
                //var data = new ExamStuRecord();
                //data.TopicID = item.id;
            }
        }
        public void PreExam()
        {
            if (examList != null)
            {
                nowItem = nowItem - 1;

            }
            examList = ListBankView.Where(x => x.index == nowItem).FirstOrDefault();

        }

        public string InsertScoreDatas(ExamPageModel model)
        {
            var guid = Guid.NewGuid().ToString();
            if (model.VExamUserInfo.LExamViews.Count() > 0)
            {
                ExamScore sc = new ExamScore();
                sc.ExamType = model.VExamUserInfo.ExamType;
                sc.CreateDate = DateTime.Now;
                sc.CreateUser = model.VExamUserInfo.UserName;
                sc.IsTest = model.VExamUserInfo.IsTest;
                sc.TatalTopicNum = model.VExamUserInfo.LExamViews.Count();
                sc.PassScore = model.VExamUserInfo.PassScore;
                //+科目
                sc.ExamSubject = model.VExamUserInfo.ExamSubject;
                sc.IsQuestion = model.VExamUserInfo.IsQuestion;

                int sd = 0;
                int score = 0;
             
                if (sc.IsQuestion == true)
                {
                    sc.CorrectScore = 0;

                }
                else
                {
                    sc.TotalScore = model.VExamUserInfo.TotalScore;
                    foreach (var item in model.VExamUserInfo.LExamViews)
                    {
                        string Remarks = string.Empty;
                        ExamRecord record = new ExamRecord();
                        //正确答案ABCD
                        var ss = item.RightKey.OrderBy(x => x).ToArray();
                        var test = string.Empty;
                        var ltest = string.Empty;
                        for (int i = 0; i < item.selectItem.Length; i++)
                        {
                            var ids = Convert.ToInt32(item.selectItem[i]);
                            test += item.ansowerList[ids].ansowerflag+";";
                            if (item.selectItem[i].Equals("0"))
                            {
                                ltest += "A;";
                            }
                            else if (item.selectItem[i].Equals("1"))
                            {
                                ltest += "B;";
                            }
                            else if (item.selectItem[i].Equals("2"))
                            {
                                ltest += "C;";
                            }

                            else if (item.selectItem[i].Equals("3"))
                            {
                                ltest += "D;";
                            }

                            else if (item.selectItem[i].Equals("4"))
                            {
                                ltest += "E;";
                            }

                        }
                        item.LselectItem = test.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        var sl = item.LselectItem.OrderBy(x => x).ToArray();
                        record.WriteAnsower = ltest;
                        var altest = ltest.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        //我选择的答案abcd
                        if (item.LselectItem != null)
                        {
                            //答对题数CorrectNum
                            if (Enumerable.SequenceEqual(ss, sl))
                            {
                                sd++;
                                sc.CorrectNum = sd;
                                //答对分数CorrectScore
                                score += item.TopicScore;
                            }
                            else
                            {
                                //错误之后找出正确答案
                                foreach (var ii in item.RightKey)
                                {
                                    if (item.ansowerList.Where(x => x.ansowerflag == ii).Count() != 0)
                                    {
                                        Remarks += item.ansowerList.Where(x => x.ansowerflag == ii).FirstOrDefault().ansower+";";
                                    }
                                }

                            }
                            if (string.IsNullOrEmpty(Remarks))
                            {
                                record.IsRight = true;
                            }
                        }
                        //题目列表ansowerList B
                        //我选择的答案item.selectItem 0,1
                        //LTEST 我选择的答案页面呈现对应的abcd
                        //test 我选择的题目背后答案abcd
                        //LselectItem我选择的题目背后答案abcd
                        //rightkey正确答案ABCD
                        #region
                        record.ExamGuid = guid;
                        record.TopicTitle = item.proName;
                        if (item.TopicTitlePic != null)
                        {
                            record.TopicTitlePicNum = item.TopicTitlePic;

                        }
                        record.TopicNum = Convert.ToInt32(item.TopicScore);
                        record.Type = item.type;
                        record.Remark = item.Remark;
                        if (item.RightKey.Count() > 0)
                        {
                            foreach (var sr in item.RightKey)
                            {
                                //正确的答案转成a;b;c;d
                                record.CorrectAnsower += sr + ';';
                            }
                        }
                        //把题目集合往选项ABC里面插入
                        if (item.ansowerList != null && item.ansowerList.Count() > 0)
                        {
                            for (int i = 0; i < item.ansowerList.Count();)
                            {
                                if (item.ansowerList[0] != null)
                                {
                                    record.OptionA = item.ansowerList[0].ansower;
                                    record.OptionAPicNum = item.ansowerList[0].ansowerpic;
                                }

                                if (item.ansowerList[1] != null)
                                {
                                    record.OptionB = item.ansowerList[1].ansower;
                                    record.OptionBPicNum = item.ansowerList[1].ansowerpic;

                                }
                                if (item.ansowerList.Count() >= 3)
                                {
                                    if (item.ansowerList[2] != null)
                                    {
                                        record.OptionC = item.ansowerList[2].ansower;
                                        record.OptionCPicNum = item.ansowerList[2].ansowerpic;

                                    }
                                }
                                if (item.ansowerList.Count() >= 4)
                                {
                                    if (item.ansowerList[3] != null)
                                    {
                                        record.OptionD = item.ansowerList[3].ansower;
                                        record.OptionDPicNum = item.ansowerList[3].ansowerpic;

                                    }
                                }
                                if (item.ansowerList.Count() >= 5)
                                {
                                    if (item.ansowerList[4] != null)
                                    {
                                        record.OptionE = item.ansowerList[4].ansower;//题目
                                        record.OptionEPicNum = item.ansowerList[4].ansowerpic;//图片
                                    }
                                }

                                i++;
                            }

                        }
                        record.DaRemark = Remarks;
                        record.CreateUser = model.VExamUserInfo.UserName;
                        record.CreateDate = DateTime.Now;
                        //插入试题
                        Data.ExamRecord.Insert_ExamRecord(record, null, new string[] { "ID" });
                        #endregion

                        #region 呈现试题
                       
                        Listexam.Add(new ExamScoreInfo
                        {
                            ExamID =guid,
                            TopicTitle = record.TopicTitle,
                            TopicNum = record.TopicNum,
                            ansowerList = item.ansowerList,
                            isright = record.IsRight,
                            Type = record.Type,
                            selectItem = item.selectItem,
                            CorrectAnsower = record.CorrectAnsower,
                            WriteItem = record.WriteAnsower,
                            TopicTitlePicNum = record.TopicTitlePicNum,
                            Remark = record.Remark,
                            TopicTitlePic = record.TopicTitlePicNum,
                            DeRemark = Remarks
                        });






                        #endregion
                    }
                }
                //插入分数记录
                if (sc.CorrectNum == null)
                { sc.CorrectNum = 0; }
                sc.CorrectScore = score;
                sc.ExamGuid = guid;
                Data.ExamScore.Insert_ExamScore(sc, null, new string[] { "ExamID" });
                VExamScore = sc;


                if (model.VExamUserInfo.IsTest == false)
                {
                    //根据人员,科目,ExamStatus更新分数,时间，isexam
                    var details = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = model.VExamUserInfo.UserName, SubjectName = model.VExamUserInfo.ExamSubject, ExamStatus = "HrCheck", IsStop = false, TypeName = model.VExamUserInfo.ExamType });
                      
                    if (details.Count()>0&&details!=null)
                    { 
                        var detail = details.Where( x=>x.UserExamDate == null).OrderByDescending(x => x.ExamDate).FirstOrDefault();
                        detail.ExamScore = score;
                        detail.UserExamDate = DateTime.Now;
                        if (model.VExamUserInfo.IsTest == true)
                        {
                            detail.IsExam = "false";
                        }
                        else
                        {
                            var ruleinfo = Data.ExamRule.Get_All_ExamRule(new { SubjectName = model.VExamUserInfo.ExamSubject });
                            if (ruleinfo.Count() > 0 && ruleinfo != null)
                            {
                                var PassScore = ruleinfo.FirstOrDefault().PassScore;
                                if (score >= PassScore)
                                {
                                    detail.IsExamPass = true;
                                    if (model.VExamUserInfo.ExamType == "电子端岗位技能津贴")
                                    {
                                        UpdateElectronicUser(model.VExamUserInfo.UserName, model.VExamUserInfo.ExamSubject);
                                    }
                                }
                                else
                                {
                                    detail.IsExamPass = false;
                                    //if (model.VExamUserInfo.ExamType == "电子端岗位技能津贴")
                                    //{
                                    //    UpdateElectronicUser(model.VExamUserInfo.UserName, model.VExamUserInfo.ExamSubject,true);
                                    //}
                                }
                            }

                            detail.IsExam = "true";
                        }
                        Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(detail, null, new string[] { "ID" });

                    }

                }


            }

            return guid;
        }

        //电子端考试成功之后update ElectronicUser 状态
        public void UpdateElectronicUser(string usercode, string ExamSubject,bool IsEnable=false)
        {
            var exam = Data.ExamUserInfo.Get_ExamUserInfo(new { UserCode = usercode, SubjectName = ExamSubject, IsEnable = 0 });
            if (exam != null)
            {
                exam.EStatus = true;
               // exam.IsEnable = IsEnable;
                Data.ExamUserInfo.Update_ExamUserInfo(exam, null, new string[] { "ID" });
            }


        }
        public void UpdateLevel(ExamPageModel model, string name, string examguid)
        {
            var usercode = "";
            var usersheet = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { EamilUsername = name });
            if (usersheet != null)
            {
                usercode = usersheet.UserCode;
            }
            var guid = Data.ExamScore.Get_All_ExamScore(new { CreateUser = usercode, ExamGuid = examguid,IsTest=false});
            if (guid.Count() > 0)
            {
                var CorrectScore = guid.FirstOrDefault().CorrectScore;
                var PassScore = guid.FirstOrDefault().PassScore;
                if (CorrectScore >= PassScore)
                {
                    var userinfos = Data.ExamUserInfo.Get_All_ExamUserInfo(new { UserCode = usercode, TypeName =VExamUserInfo.ExamType});
                    if (userinfos.Count() > 0 && userinfos != null)
                    {
                        foreach (var item in userinfos)
                        {
                            if (!string.IsNullOrEmpty(item.ApplicationLevel))
                            {
                                if (model.VExamUserInfo.ExamType == "职等考试")
                                {
                                    var applevel = item.ApplicationLevel;
                                    if (!string.IsNullOrEmpty(applevel))
                                    {
                                        if (Convert.ToInt32(applevel.Substring(1, 1)) < 3)
                                        {
                                            int ss = Convert.ToInt32(applevel.Substring(1, 1)) + 1;
                                            item.ApplicationLevel = "A" + ss.ToString();
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(item.RankName))
                                    {
                                        if (Convert.ToInt32(item.RankName.Substring(item.RankName.Length - 1, 1)) < 3)
                                        {
                                            int ran = Convert.ToInt32(item.RankName.Substring(item.RankName.Length - 1, 1)) + 1;
                                            item.RankName = "A-" + ran;
                                        }
                                        if (item.RankName.Contains("A-3") && item.PostName=="作业员")
                                        {
                                            item.PostName = "技术员";
                                        }
                                    }
                                }
                                else if (model.VExamUserInfo.ExamType == "关键岗位技能等级")
                                {
                                    if (Convert.ToInt32(item.ApplicationLevel.Substring(1, 1)) < 8)
                                    {
                                        int ss = Convert.ToInt32(item.ApplicationLevel.Substring(1, 1)) + 1;
                                        item.ApplicationLevel = item.ApplicationLevel.Substring(0, 1) + ss.ToString();
                                        item.Achievement = null;
                                    }
                                }
                                else if (model.VExamUserInfo.ExamType == "Chassis技能等级考试")
                                {
                                    if (item.ApplicationLevel=="中级")
                                    {
                                        item.ApplicationLevel = "高级";
                                        item.Achievement = null;
                                    }
                                }
                                Data.ExamUserInfo.Update_ExamUserInfo(item, null, new string[] { "ID" });
                            }
                          
                        }
                    }
                }

            }


        }
    }
    //第二层
    public class ExamView
    {
        public string id { get; set; }
        public string[] selectItem { get; set; } //选择题自己选的用户答案 type为0时 类型为''   type为1时 类型为[]   type为2时 类型为''
        public string type { get; set; }//0 单选题  1 多选题  2 问答题 题目类型
        public string proName { get; set; }//题目名称
        public List<LAnsower> ansowerList { get; set; }//题目选项
        public string[] RightKey { get; set; }//正确答案
        public int TopicScore { get; set; }//单题分数
        public int index { get; set; }
        public string Remark { get; set; }
        public string[] LselectItem { get; set; }
        public string ExamTypeName { get; set; }//题目类型中文
        public string TopicTitlePic { get; set; }
        public string TopicLevel { get; set; }//G1 G2
        public string TopicMajor { get; set; }//c#db
        //public string ExamType { get; set; }

        //public bool IsTest { get; set; }

        //selectItem: '',   //用户答案 type为0时 类型为''   type为1时 类型为[]   type为2时 类型为''
        //            type: 0,      //0 单选题  1 多选题  2 问答题
        //            proName: '在 HTML 页面中包含一个按钮控件 mybutton ，如果要实现点击该按钮时调用已定义的 JavaScript 函数 compute ，要编写的 HTML 代码是()',
        //            ansower: 4,     //当前题目正确答案
        //            ansowerList: ['<input name=”mybutton” type=”button” onBlur=”compute()” value=”计算”>', '<input name=”mybutton” type=”button” onFocus=”compute()” value=”计算”>', '<input name=”mybutton” type=”button” onClick=”function compute()” value=”计算”>', '<input name=”mybutton” type=”button” onClick=”compute()” value=”计算”>']
    }
    public class LAnsower
    {
        public string TopicType { get; set; }//题目类型
        public string ansower { get; set; }//选项内容
        public string ansowerpic { get; set; }//选项图片
        public string ansowerflag { get; set; }//答案选项A
    }
    public class ExamBankView
    {
        public int ID { get; set; }

        public decimal TotalScore { get; set; }
        public string TopicType { get; set; }//问答

        public string TopicTitle { get; set; }

        public string TopicTitlePicNum { get; set; }

        public string[] RightKey { get; set; }

        public string Remark { get; set; }

        public string OptionA { get; set; }

        public string OptionAPicNum { get; set; }

        public string OptionB { get; set; }

        public string OptionBPicNum { get; set; }

        public string OptionC { get; set; }

        public string OptionCPicNum { get; set; }

        public string OptionD { get; set; }

        public string OptionDPicNum { get; set; }

        public string OptionE { get; set; }

        public string OptionEPicNum { get; set; }

        public string OptionF { get; set; }

        public string OptionFPicNum { get; set; }
        public int Score { get; set; }
        public string ExamTypeName { get; set; }
        public string TopicTitlePic { get; set; }
        public string TopicLevel { get; set; }//G1 G2
        public string TopicMajor { get; set; }//c#db

    }
    public class ExamUserInfo
    {
        public string ExamType { get; set; }//考试类型
        public string ExamSubject { get; set; }
        public bool IsTest { get; set; }//是否测试
        public bool IsRead { get; set; }//是否人工审批
        public decimal TotalScore { get; set; }//总分
        public string UserName { get; set; }//考试人
        public string UserTextName { get; set; }//考试人姓名
        public string StartDesc { get; set; }//开头
        public string EndDesc { get; set; }
        public decimal TotalTime { get; set; }
        public List<ExamView> LExamViews { get; set; }//题目选项内容
        public decimal PassScore { get; set; }
        public bool IsQuestion { get; set; }
    }
}