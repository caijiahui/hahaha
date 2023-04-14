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
        public List<string> ListWorkPlace { get; set; }
        public List<string> LExamType { get; set; }
        public List<string> LDepartCode { get; set; }
        public List<string> LSubject { get; set; }
        public ExamConfirmModel() : base()
        {
            LstUserInfos = new List<UserInfos>();
            ListWorkPlace = new List<string>();
            LExamType = new List<string>();
            LDepartCode = new List<string>();
            LSubject = new List<string>();
        }
     
        public void GetExamInfo(SearchHrData data)
        {
            var ddate = Convert.ToDateTime(data.ExamDate);
            var examdetail = ddate.AddDays(1);
            var lst = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { IsStop =0, IsExam = "false" });
            if (!string.IsNullOrEmpty(data.WrokPlace))
            { lst = lst.Where(x => x.OrgName == data.WrokPlace).ToList(); }
            if (!string.IsNullOrEmpty(data.TypeName))
            {
                if (data.TypeName != "全部")
                    lst = lst.Where(x => x.TypeName == data.TypeName).ToList();
            }
            if (!string.IsNullOrEmpty(data.SubjectName))
            { lst = lst.Where(x => x.TypeName == data.SubjectName).ToList(); }
            if (!string.IsNullOrEmpty(data.ExamDate))
            { lst = lst.Where(x => x.ExamDate>= ddate && x.ExamDate< examdetail).ToList(); }
            if (!string.IsNullOrEmpty(data.DepartCode))
            { lst = lst.Where(x => x.DepartCode == data.DepartCode).ToList(); }
            if (!string.IsNullOrEmpty(data.UserCode))
            {   data.UserCode = data.UserCode.ToUpper();
                lst = lst.Where(x => x.UserCode.Contains(data.UserCode)).ToList(); }
            if (!string.IsNullOrEmpty(data.UserName))
            { lst = lst.Where(x => x.UserName.Contains(data.UserName)).ToList(); }
            LstUserInfos = lst.Select(y => new UserInfos
            {
                DepartCode = y.DepartCode,
                UserCode = y.UserCode,
                UserName = y.UserName,
                SubjectName = y.SubjectName,
                TypeName = y.TypeName,
                OrgName = y.OrgName,
                IsStartExam = y.IsStartExam,
                ExamDate=y.ExamDate,
                ID = y.ID
            }).OrderByDescending(t => t.ExamDate).ToList();

            var user = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo();
            ListWorkPlace = user.GroupBy(x => x.OrgName).Select(y => y.Key).Distinct().ToList();
            LExamType.Add("全部");
            foreach (var item in Data.ExamType.Get_All_ExamType().GroupBy(x => x.TypeName).Select(y => y.Key))
            {
                LExamType.Add(item);
            }
            
        }
        public void SaveConfirmUser(int ID,string username)
        {
            var ddate =Convert.ToDateTime(DateTime.Now.ToShortDateString());
            var examdetail = ddate.AddDays(1);
            var user = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ID=ID });
            if (user.Count() > 0 && user != null)
            {
                foreach (var item in user)
                {
                    if (item.ExamDate< examdetail&&item.ExamDate> ddate)
                    {
                        item.IsStartExam = true;
                        item.StartExamUser = username;
                        item.StartExamDate = DateTime.Now;
                        Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(item, null, new string[] { "ID" });
                    }                   
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
        public void GetDepartcode(string code)
        {
            var lst = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo();
            LDepartCode = lst.Where(x=>x.OrgName==code).GroupBy(x => x.DepartCode).Select(y => y.Key).Distinct().ToList();
        }
        public void GetTypeSubject(string code)
        {
            var lst = Data.ExamSubject.Get_All_ExamSubject(new { TypeName =code});
            LSubject = lst.GroupBy(x => x.SubjectName).Select(y => y.Key).Distinct().ToList();
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