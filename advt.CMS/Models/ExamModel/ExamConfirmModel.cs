using advt.Entity;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class ExamConfirmModel
    {
        public List<UserInfos> LstUserInfos { get; set; }

        public ExamConfirmModel() : base()
        {
            LstUserInfos = new List<UserInfos>();
        }
        public void ExamInfo(string name)
        {
            var use = Data.ExamUserDetailInfo.GetAuthority(name);
            if (use.Count() > 0&&use!=null)
            {
                var ddate = DateTime.Now.Date;
                var examdetail = ddate.AddDays(1);
                var lst = Data.ExamUserDetailInfo.Get_All_ExamInfo(ddate, examdetail);               
                LstUserInfos = lst.Select(y => new UserInfos
                {
                    DepartCode = y.DepartCode,
                    UserCode = y.UserCode,
                    UserName = y.UserName,
                    SubjectName = y.SubjectName,
                    TypeName = y.TypeName,
                    OrgName = y.OrgName,
                    IsStartExam = y.IsStartExam,
                    ID=y.ID
                }).OrderByDescending(t => t.ExamDate).ToList();
            }
        }
        public void SaveConfirmUser(int ID,string username)
        {
            var user = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ID=ID });
            if (user.Count() > 0 && user != null)
            {
                foreach (var item in user)
                {
                    item.IsStartExam = true;
                    item.StartExamUser = username;
                    item.StartExamDate = DateTime.Now;
                    Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(item, null, new string[] { "ID" });
                }                
            }
        }
        public bool GetAdminType(string name)
        {
            bool isadmin = false;
            var use = Data.ExamUserDetailInfo.GetAuthority(name);
            if (use.Count() > 0 && use != null)
            {
                isadmin = true;
            }
            return isadmin;
        }

    }
    public class UserInfos
    { 
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string RuleName { get; set; }
        public string DepartCode { get; set; }
        public string SubjectName { get; set; }
        public string TypeName { get; set; }
        public DateTime? ExamDate { get; set; }
        public string OrgName { get; set; }
        public bool IsStartExam { get; set; }//是否签到
        public int ID { get; set; }

    }
   
}