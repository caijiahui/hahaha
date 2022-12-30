using advt.Entity;
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
            //根据角色判断 是hr还是admin还是其他人员
            var use = Data.ExamUserDetailInfo.GetAuthority(name);
            if (use.Count() > 0&&use!=null)
            {
                var ddate = DateTime.Now.Date;
                var examdetail = ddate.AddDays(1);
                var lst = Data.ExamUserDetailInfo.Get_All_ExamInfo(ddate, examdetail);
                if (lst.Count > 0 && lst != null)
                {
                    foreach (var item in lst)
                    {
                        LstUserInfos.Add(new UserInfos
                        {
                            DepartCode = item.DepartCode,
                            UserCode = item.UserCode,
                            UserName = item.UserName,
                            RuleName = item.RuleName,
                            SubjectName = item.SubjectName,
                            TypeName = item.SubjectName,
                            ExamDate = item.ExamDate
                        });
                    }
                }
            }

        }
        public void SaveConfirmUser(ExamConfirmModel model,string username) { 
        
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

    }
   
}