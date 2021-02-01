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
        public ExamFinishModel() : base()
        {
            VexamRecord = new ExamRecord();
            VExamScore = new ExamScore();
            Listexam = new ExamScoreInfo();
            examList = new List<ExamScoreInfo>();
            ListVexamRecord = new List<ExamRecord>();
            Listexam.ansowerList = new List<LAnsower>();
            if (Data.ExamScore.Get_All_ExamScore().Count() > 0)
            {
                var ss = Data.ExamScore.Get_All_ExamScore().Max(x => x.ExamID);
                VExamScore = Data.ExamScore.Get_ExamScore(ss);
           
                ListVexamRecord = Data.ExamRecord.Get_All_ExamRecord(ss);
                ListVexamRecord = ListVexamRecord.Where(x => x.ExamID == ss.ToString()).ToList();               
                ID = ss;
            }
           
        }
        public void GetExamListInfo(int id)
        {        
            if (id != 0)
            {                
                foreach (var item in ListVexamRecord)
                {
                    var output = new string[] { };
                    var list= new string[] { };
                    bool isright = false;
                    var option = "";
                    if (item.CorrectAnsower == item.WriteAnsower)
                    {
                        isright = true;
                    }
                    List<LAnsower> answ = new List<LAnsower>();
                    if (item.OptionA != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionA,
                            ansowerflag = item.CorrectAnsower
                        });
                    }
                    if (item.OptionB != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionB,
                            ansowerflag = item.CorrectAnsower
                        });
                    }
                    if (item.OptionC != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionC,
                            ansowerflag = item.CorrectAnsower
                        });
                    }
                    if (item.OptionD != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionD,
                            ansowerflag = item.CorrectAnsower
                        });
                    }
                    if (item.OptionE != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionE,
                            ansowerflag = item.CorrectAnsower
                        });
                    }
                    if (item.OptionF != null)
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionF,
                            ansowerflag = item.CorrectAnsower
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
                        ExamID = id.ToString(),
                        TopicTitle = item.TopicTitle,
                        TopicNum = item.TopicNum,
                        ansowerList= answ,
                        isright= isright,
                        Type=item.Type,
                        selectItem=output,
                        selectOption=option,
                        CorrectAnsower=item.CorrectAnsower,
                        WriteItem=item.WriteAnsower
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
    }
   

}