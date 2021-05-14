using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using advt.Entity;

namespace advt.CMS.Models
{
    public class ExamFinishModel
    {
        public ExamRecord VexamRecord { get; set; }
        public ExamScore VExamScore { get; set; }
        public decimal totalGrade { get; set; }
        public decimal grade { get; set; }
        public int rightItem { get; set; }
        public int total { get; set; }
        public ExamScoreInfo Listexam { get; set; }
        public List<ExamScoreInfo> examList { get; set; }
        public List<ExamRecord> ListVexamRecord { get; set; }
        public int ID { get; set; }
        public int ExamID { get; set; }
        public List<advt_user_sheet> Listadvt_user_sheet { get; set; }
        public ExamFinishModel() : base()
        {
            VexamRecord = new ExamRecord();
            VExamScore = new ExamScore();
            Listexam = new ExamScoreInfo();
            examList = new List<ExamScoreInfo>();
            ListVexamRecord = new List<ExamRecord>();
            Listexam.ansowerList = new List<LAnsower>();


        }

        public void GetCsore(int ExamId)
        {
            if (Data.ExamScore.Get_All_ExamScore().Count() > 0)
            {
                ExamID = ExamId;
                var ss = Data.ExamScore.Get_All_ExamScore(new { ExamID = ExamId }).Max(x => x.ExamID);

                VExamScore = Data.ExamScore.Get_ExamScore(ss);

                ListVexamRecord = Data.ExamRecord.Get_All_ExamRecord(ss);
                ListVexamRecord = ListVexamRecord.Where(x => x.ExamID == ss.ToString()).ToList();
                ID = ss;
            }
        }
        public void GetExamListInfo(int id, string name,string test,string subjectname)
        {
            var usercode = "";
            if (Data.ExamScore.Get_All_ExamScore().Count() > 0)
            {
                var usersheet = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { EamilUsername = name });
                if (usersheet != null)
                {
                    usercode = usersheet.UserCode;
                }
                bool be = false;
                if (test == "True")
                { be = true; }
                var ss = Data.ExamScore.Get_All_ExamScore(new { CreateUser = usercode, IsTest = be }).Max(x => x.ExamID);

                VExamScore = Data.ExamScore.Get_ExamScore(ss);

                ListVexamRecord = Data.ExamRecord.Get_All_ExamRecord(ss);
                ListVexamRecord = ListVexamRecord.Where(x => x.ExamID == ss.ToString()).ToList();
                ID = ss;
            }
            if (ID != 0)
            {
                foreach (var item in ListVexamRecord.Where(x => x.ExamID == ID.ToString()))
                {
                    var output = new string[] { };
                    var list = new string[] { };

                    var option = "";
                    //if (item.CorrectAnsower == item.HideAnsower)
                    //{
                    //    isright = true;
                    //}
                    List<LAnsower> answ = new List<LAnsower>();
                    if (item.OptionA != null || item.OptionAPicNum != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionA,
                            ansowerflag = item.CorrectAnsower,
                            ansowerpic = item.OptionAPicNum
                        });
                    }
                    if (item.OptionB != null || item.OptionBPicNum != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionB,
                            ansowerflag = item.CorrectAnsower,
                            ansowerpic = item.OptionBPicNum
                        });
                    }
                    if (item.OptionC != null || item.OptionCPicNum != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionC,
                            ansowerflag = item.CorrectAnsower,
                            ansowerpic = item.OptionCPicNum
                        });
                    }
                    if (item.OptionD != null || item.OptionDPicNum != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionD,
                            ansowerflag = item.CorrectAnsower,
                            ansowerpic = item.OptionDPicNum
                        });
                    }
                    if (item.OptionE != null || item.OptionEPicNum != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionE,
                            ansowerflag = item.CorrectAnsower,
                            ansowerpic = item.OptionEPicNum
                        });
                    }
                    if (item.OptionF != null || item.OptionFPicNum != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionF,
                            ansowerflag = item.CorrectAnsower,
                            ansowerpic = item.OptionFPicNum
                        });
                    }
                    if (item.Type == "0")
                    {
                        if (item.WriteAnsower != null)
                        {
                            list = item.WriteAnsower.Split(';');
                            option = list.FirstOrDefault();
                            var ss = "";
                            if (!string.IsNullOrEmpty(option))
                            {
                                if (option.Equals("A"))
                                {
                                    ss = "0";
                                }
                                else if (option.Equals("B"))
                                {
                                    ss = "1";
                                }
                                else if (option.Equals("C"))
                                {
                                    ss = "2";
                                }
                                else if (option.Equals("D"))
                                {
                                    ss = "3";
                                }
                                else if (option.Equals("E"))
                                {
                                    ss = "4";
                                }
                                option = ss;


                            }
                        }
                    }
                    else if (item.Type == "1")
                    {
                        if (item.WriteAnsower != null)
                        {
                            var listnum = "";
                            output = item.WriteAnsower.Split(';');
                            var tt = "";
                            foreach (var w in output)
                            {
                                if (!string.IsNullOrEmpty(w))
                                {
                                    if (w.Equals("A"))
                                    {
                                        tt = "0";
                                    }
                                    else if (w.Equals("B"))
                                    {
                                        tt = "1";
                                    }
                                    else if (w.Equals("C"))
                                    {
                                        tt = "2";
                                    }
                                    else if (w.Equals("D"))
                                    {
                                        tt = "3";
                                    }
                                    else if (w.Equals("E"))
                                    {
                                        tt = "4";
                                    }

                                    listnum += tt + ';';
                                }

                            }
                            output = listnum.Split(';');
                        }
                    }

                    examList.Add(new ExamScoreInfo
                    {
                        ExamID = ID.ToString(),
                        TopicTitle = item.TopicTitle,
                        TopicNum = item.TopicNum,
                        ansowerList = answ,
                        isright = item.IsRight,
                        Type = item.Type,
                        selectItem = output,
                        selectOption = option,
                        CorrectAnsower = item.CorrectAnsower,
                        WriteItem = item.WriteAnsower,
                        TopicTitlePicNum = item.TopicTitlePicNum,
                        Remark = item.Remark,
                        TopicTitlePic = item.TopicTitlePicNum,
                        DeRemark = item.DaRemark
                    });
                }
            }
        }
    }
    public class ExamScoreInfo
    {
        public string ExamID { get; set; }

        public string TopicTitle { get; set; }

        public string TopicTitlePicNum { get; set; }

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

        public DateTime? CreateDate { get; set; }

        public string CreateUser { get; set; }
        public decimal TopicNum { get; set; }
        public string CorrectAnsower { get; set; }

        public List<LAnsower> ansowerList { get; set; }//题目选项
        public bool isright { get; set; }
        public string Type { get; set; }
        public string[] selectItem { get; set; }
        public string selectOption { get; set; }
        public string WriteItem { get; set; }
        public string Remark { get; set; }
        public string TopicTitlePic { get; set; }
        public string DeRemark { get; set; }
    }


}