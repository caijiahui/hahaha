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
        }

        public void GetListExam()
        {
            try
            {
               
                var ListBanks = new List<ExamBankView>();
                var Rule = Data.ExamRule.Get_ExamRule(26);
                if (Rule != null)
                {
                    VExamUserInfo.ExamType = Rule.TypeName;//考试名称
                    VExamUserInfo.TotalScore = Rule.TotalScore;//总分
                    VExamUserInfo.UserName = "Q-19568";//工号
                    VExamUserInfo.IsTest = true;//是否模拟
                    VExamUserInfo.IsRead = Rule.IsRead;//是否审批
                    VExamUserInfo.TotalTime = Rule.TotalTime;
                    VExamUserInfo.StartDesc = Rule.StartDeac;
                    VExamUserInfo.EndDesc = Rule.EndDesc;
                    VExamUserInfo.PassScore = Rule.PassScore;
                    var topicnum = Data.ExamRuleTopicType.Get_All_ExamRuleTopicType_RuleId(Rule.ID);
                    foreach (var item in topicnum)
                    {
                        var banks = Data.ExamBank.Get_All_ExamBank_ExamType_Rule(item.TopicType, item.TopicMajor, item.TopicLevel);
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
                                OptionE = items.OptionEPicNum,
                                OptionF = items.OptionF,
                                OptionEPicNum = items.OptionEPicNum,
                                OptionFPicNum = items.OptionFPicNum,
                                Score = Convert.ToInt32(item.TopicScore),
                                TotalScore = Convert.ToDecimal(item.TopicScore),
                                RightKey = right,
                                Remark=items.Remark
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
                        ListBankView.Add(new ExamView
                        {
                            id = item.ID.ToString(),
                            type = item.TopicType,//题目类型
                            proName = item.TopicTitle,//题目标题
                            ansowerList = answ,//选项内容List
                            index = index,//下标
                            RightKey = item.RightKey,//正确答案
                            Remark = item.Remark,//答案解析
                            TopicScore= item.Score//单题分数

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
        public ExamUserInfo TestData()
        {
         
            TestD.LExamViews = new List<ExamView>();
            var LAnsower = new List<LAnsower>();
            TestD.IsTest = false;
            TestD.ExamType = "技能等级考试";
            TestD.TotalScore = 100;//总分
            TestD.UserName = "Jiahui";
            //单选
            string[] selectItem = { "B" };
            LAnsower.Add(new LAnsower
            {

                TopicType = "0",//单选题
                ansower = "10>=a>=0",
                ansowerpic = "",
                ansowerflag = "A"
            });
            LAnsower.Add(new LAnsower
            {
                TopicType = "0",//单选题
                ansower = "a>=0 and a<=10",
                ansowerpic = "",
                ansowerflag = "B"
            });
            LAnsower.Add(new LAnsower
            {
                TopicType = "0",//单选题
                ansower = "a1223232",
                ansowerpic = "",
                ansowerflag = "C"
            });
            LAnsower.Add(new LAnsower
            {
                TopicType = "0",//单选题
                ansower = "a2",
                ansowerpic = "",
                ansowerflag = "D"
            });
            LAnsower.Add(new LAnsower
            {
                TopicType = "0",//单选题
                ansower = "a3",
                ansowerpic = "",
                ansowerflag = "D"
            });
            TestD.LExamViews.Add(new ExamView
            {
                TopicScore = 5,
                RightKey = new string[] { "B"},//正确答案
                proName = "C# 中能正确表示逻辑关系：“10大于等于a大于等于0”的C语言表达式是",//题目内容
                type = "0",//题目类型
                selectItem = selectItem,//自己选择题选择的内容
                ansowerList = LAnsower//题目选项信息
            });
            //多选题
            selectItem = new string[] { "A", "C" };
            LAnsower = new List<LAnsower>();
            LAnsower.Add(new LAnsower
            {

                TopicType = "1",//多选题
                ansower = "10>=a>=0",
                ansowerpic = "",
                ansowerflag = "A"
            });
            LAnsower.Add(new LAnsower
            {
                TopicType = "1",//多选题
                ansower = "a>=0 and a<=10",
                ansowerpic = "",
                ansowerflag = "B"
            });
            LAnsower.Add(new LAnsower
            {
                TopicType = "1",//多选题
                ansower = "a1223232",
                ansowerpic = "",
                ansowerflag = "C"
            });
            LAnsower.Add(new LAnsower
            {
                TopicType = "1",//多选题
                ansower = "a2",
                ansowerpic = "",
                ansowerflag = "D"
            });
            LAnsower.Add(new LAnsower
            {
                TopicType = "1",//多选题
                ansower = "a3",
                ansowerpic = "",
                ansowerflag = "D"
            });
            TestD.LExamViews.Add(new ExamView
            {
                TopicScore = 5,
                RightKey = new string[] { "A", "C" },//正确答案
                proName = "C# 中能正确表示逻辑关系：“10大于等于a大于等于0”的C语言表达式是",//题目内容
                type = "1",//题目类型
                selectItem = selectItem,//自己选择题选择的内容
                ansowerList = LAnsower//题目选项信息
            });
            //填空题
            var ans = "瞎填";
            TestD.LExamViews.Add(new ExamView
            {
                TopicScore = 5,
                RightKey = new string[] { "C", "D" },//正确答案
                proName = "C# 中能正确表示逻辑关系：“10大于等于a大于等于0”的C语言表达式是",//题目内容
                type = "2",//题目类型
                selectItem = selectItem,//自己选择题选择的内容
                ansowerList = LAnsower//题目选项信息
            });
            return TestD;
        }

        public void InsertScoreData(ExamPageModel model)
        {
            if (model.TestD.LExamViews.Count() > 0)
            {
                ExamScore sc = new ExamScore();
                sc.ExamType = model.TestD.ExamType;
                sc.TotalScore = model.TestD.TotalScore;
                sc.CreateDate = DateTime.Now;
                sc.CreateUser = model.TestD.UserName;
                sc.IsTest = model.TestD.IsTest;
                sc.TatalTopicNum = model.TestD.LExamViews.Count();
              
                int sd = 0;
                int score = 0;
                foreach (var item in model.TestD.LExamViews)
                {
                    
                    var ss = item.RightKey.OrderBy(x => x).ToArray();
                    var sl = item.selectItem.OrderBy(x => x).ToArray();
                    //答对题数CorrectNum
               
                    if (item.selectItem.Count() > 0)
                    {
                        if (Enumerable.SequenceEqual(ss, sl))
                        {
                            sd++;
                            sc.CorrectNum = sd;
                            //答对分数CorrectScore
                            score += item.TopicScore;
                        }
                    }
                }
                sc.CorrectScore = score;
                Data.ExamScore.Insert_ExamScore(sc, null, new string[] { "ExamID" });
                
            }
        }
        public void InsertRecoredData(ExamPageModel model)
        {
            var ee = Data.ExamScore.Get_All_ExamScore().Max(x=>x.ExamID);
            foreach (var item in model.TestD.LExamViews)
            {
                ExamRecord record = new ExamRecord();

                record.ExamID = ee.ToString();
                record.TopicTitle = item.proName;
                record.TopicNum = Convert.ToInt32(item.TopicScore);
                record.Type = item.type;
                if (item.selectItem.Count() > 0)
                {
                    foreach (var ss in item.selectItem)
                    {
                        //选择的答案
                        record.WriteAnsower += ss + ';';
                    }
                }
                if (item.RightKey.Count() > 0)
                {
                    foreach (var sr in item.RightKey)
                    {
                        //选择的答案
                        record.CorrectAnsower += sr + ';';
                    }
                }
                foreach (var items in item.ansowerList)
                {
                    if (items.ansowerflag == "A")
                    {
                        record.OptionA = items.ansower;
                    }
                    if (items.ansowerflag == "B")
                    {
                        record.OptionB = items.ansower;
                    }
                    if (items.ansowerflag == "C")
                    {
                        record.OptionC = items.ansower;
                    }
                    if (items.ansowerflag == "D")
                    {
                        record.OptionD = items.ansower;
                    }
                    if (items.ansowerflag == "E")
                    {
                        record.OptionE = items.ansower;
                    }
                    if (items.ansowerflag == "F")
                    {
                        record.OptionF = items.ansower;
                    }
                }
                record.CreateUser = model.TestD.UserName;
                record.CreateDate = DateTime.Now;
                Data.ExamRecord.Insert_ExamRecord(record, null, new string[] { "ID" });

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

    }
    public class ExamUserInfo
    {
        public string ExamType { get; set; }//考试科目
        public bool IsTest { get; set; }//是否测试
        public bool IsRead { get; set; }//是否人工审批
        public decimal TotalScore { get; set; }//总分
        public string UserName { get; set; }//考试人
        public string StartDesc { get; set; }//开头
        public string EndDesc { get; set; }
        public decimal TotalTime { get; set; }
        public List<ExamView> LExamViews { get; set; }//题目选项内容
        public decimal PassScore { get; set; }
    }
}