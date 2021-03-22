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

namespace advt.CMS.Models.ExamModel
{
    public class ExamUserSubject
    {
        public List<UserSubjectModel> ListUsersubject;
        public string usercode { get; set; }
        public ExamUserSubject() : base()
        {
            ListUsersubject = new List<UserSubjectModel>();
        }
        public void GetListUsersubject(string username)
        {
            var usersheet = Data.advt_user_sheet.Get_advt_user_sheet(new { UserAccount = username});
            if (usersheet != null)
            {
                usercode = usersheet.UserCode;
            }
            var data = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrCheck", IsStop =false, UserCode = usercode });
            var group = data.GroupBy(p => new { p.TypeName }).Select(a => new { ExamType = a.Key.TypeName });
            foreach (var item in group)
            {
                var Lsubject = data.Where(x => x.TypeName == item.ExamType).ToList().Select(d=>new UserSubjectRule { ExamSubject= d.SubjectName,RuleName= d.RuleName }).ToList();
                ListUsersubject.Add(new UserSubjectModel
                {
                    ExamType = item.ExamType,
                    LExamSubject= Lsubject
                });
            }
        }
    }
    public class UserSubjectModel
    {
        public string ExamType { get; set; }//考试类型
        public List<UserSubjectRule> LExamSubject { get; set; }//考试科目
        public string RuleName { get; set; }
    }
    public class UserSubjectRule
    {
        public string ExamSubject { get; set; }
        public string RuleName { get; set; }
    }
}