using advt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class HrAuditModel
    {
        public List<ExamUserDetailInfo> ListHrAuditUser { get; set; }
        public List<ExamUserDetailInfo> ListHrAuditSuccessUser { get; set; }
        public List<ExamUserDetailInfo> LExamUserDetailInfo { get; set; }
        public List<KeyValuePair<string, string>> LExamType { get; set; }
        public List<PracticeInfo> LPracticeInfo { get; set; }
        public HrAuditModel() : base()
        {
            ListHrAuditUser = new List<ExamUserDetailInfo>();
            ListHrAuditSuccessUser = new List<ExamUserDetailInfo>();
            LExamUserDetailInfo = new List<ExamUserDetailInfo>();
            LPracticeInfo = new List<PracticeInfo>();
            LExamType = new List<KeyValuePair<string, string>>();
        }
        public void GetHrAuditUser(string typename="")
        {
            if (string.IsNullOrEmpty(typename))
            {
                ListHrAuditUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup" }).OrderByDescending(x => x.TypeName).ToList();
                ListHrAuditSuccessUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrCheck", IsStop=false }).OrderByDescending(x => x.TypeName).ToList();
            }
            else
            {
                ListHrAuditUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup",TypeName= typename }).OrderByDescending(x => x.TypeName).ToList();
                ListHrAuditSuccessUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrCheck", TypeName = typename, IsStop=false }).OrderByDescending(x => x.TypeName).ToList();
            }
            LExamType.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamType.Get_All_ExamType())
            {
                LExamType.Add(new KeyValuePair<string, string>(item.TypeName, item.TypeName));
            }
        }
        public void UpdateHrAduitUser(List<ExamUserDetailInfo> model, string username)
        {
            try
            {
                foreach (var item in model)
                {
                    item.DirectorCreateUser = username;
                    item.DirectorCreateDate = DateTime.Now;
                    Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(item, null, new string[] { "ID" });
                }
                GetHrAuditUser();
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public void StopHrAuditUser(string id,string username)
        {
            try
            {
                var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { ID = id });
                c.IsStop = true;
                c.StopCreateDate=DateTime.Now;
                c.StopCreateUser = username;
                Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(c, null, new string[] { "ID" });
                GetHrAuditUser();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SearchPracticeUserDetail(string code)
        {
            LExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = code });
            LPracticeInfo = Data.PracticeInfo.Get_All_PracticeInfo(new { UserCode = code }).OrderByDescending(x => x.CreateDate).ToList();
        }

    }
}