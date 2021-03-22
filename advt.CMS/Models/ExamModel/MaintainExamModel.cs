
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using advt.Entity;
namespace advt.CMS.Models.ExamModel
{
    public class MaintainExamModel
    {
        public ExamScore Score { get; set; }
        public List<ExamScore> ListExamScore { get; set; }

        public List<advt_user_sheet> Listadvt_user_sheet { get; set; }
        public string Name { get; set; }
        public string UserCode { get; set; }

        public MaintainExamModel() : base()
        {
            Score = new ExamScore();
            ListExamScore = new List<ExamScore>();
            Listadvt_user_sheet = new List<advt_user_sheet>();
        }
        public void GetUserScore(string code)
        {
            ListExamScore = Data.ExamScore.Get_All_ExamScore(new { CreateUser = code });
        }
        public string GetUserCode(string username)
        {
            Name = username;
               var code = "";
            Listadvt_user_sheet = Data.advt_user_sheet.Get_All_advt_user_sheet(new { UserAccount = username });
            if (Listadvt_user_sheet.Count() > 0 && Listadvt_user_sheet != null)
            {
                code = Listadvt_user_sheet.FirstOrDefault().UserCode;
            }
            UserCode = code;
            return code;
        }
    }
    }
