
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
        public string Name { get; set; }
        public string UserCode { get; set; }

        public MaintainExamModel() : base()
        {
            Score = new ExamScore();
            ListExamScore = new List<ExamScore>();
        }
        public void GetUserScore(string code)
        {
            ListExamScore = Data.ExamScore.Get_All_ExamGetScore(code,false);
        }
        public string GetUserCode(string username)
        {
            Name = username;
               var code = "";
            var user = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { UserCode = username });
            if (user != null)
            {
                UserCode = user.UserCode;
            }
            return code;
        }
    }
    }
