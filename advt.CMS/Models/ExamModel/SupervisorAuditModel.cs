using advt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class SupervisorAuditModel
    {
        public List<UserInfo> ListDirectorUserInfos { get; set; }
        public List<ExamRule> LRules { get; set; }
        public List<ExamUserDetailInfo> LExamUserDetailInfo { get; set; }
        public List<ExamUserDetailInfo> LSignedupUser { get; set; }
        public List<PracticeInfo> LPracticeInfo { get; set; }
        public List<ExamUserDetailInfo> LCheckAudtiUser { get; set; }
        public SupervisorAuditModel() : base()
        {
            ListDirectorUserInfos = new List<UserInfo>();
            LRules = new List<ExamRule>();
            LExamUserDetailInfo = new List<ExamUserDetailInfo>();
            LPracticeInfo = new List<PracticeInfo>();
            LCheckAudtiUser = new List<ExamUserDetailInfo>();
            LSignedupUser = new List<ExamUserDetailInfo>();
        }
        public void GetAllExamUserDetailInfo(string username=null)
        {
            try
            {
                var usersheets = Data.advt_user_sheet.Get_advt_user_sheet_UserJobTitle(username);
                //Hr报名 HrSignUp   主管审核Signup  hr审核 HrCheck

                //一个人可能是多个部门主管，所以循环
                foreach (var sheets in usersheets)
                {
                    //找到该主管下的部门人员
                    var groups = Data.advt_user_sheet.Get_All_advt_user_sheet(new { UserCostCenter = sheets.UserCostCenter });
                    foreach (var item in groups)
                    {
                        //找到该部门下报名人员
                        var data = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrSignUp", IsStop = false, UserCode = item.UserCode });
                        if (data != null && data.Count() != 0)
                        {
                            LCheckAudtiUser.AddRange(data);
                        }
                        var auditdata = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup", IsStop = false, UserCode = item.UserCode });
                        if (auditdata != null && auditdata.Count() != 0)
                        {
                            LSignedupUser.AddRange(auditdata);
                        }
                    }
                }
                if (LCheckAudtiUser.Count() != 0)
                {
                    var LTypename = LCheckAudtiUser.GroupBy(x => x.TypeName).Select(y => new { typename = y.Key });
                    foreach (var item in LTypename)
                    {
                        var rule = Data.ExamRule.Get_All_TypeNameExamRule(item.typename);
                        LRules.AddRange(rule);
                    }
                }
                LCheckAudtiUser = LCheckAudtiUser.OrderByDescending(x => x.TypeName).ToList();
                //主管需要审核的人员
                LSignedupUser = LSignedupUser.OrderByDescending(x => x.TypeName).ThenBy(x => x.DepartCode).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public void SearchPracticeInfo(string code)
        {
            LPracticeInfo = Data.PracticeInfo.Get_All_PracticeInfo(new { UserCode = code }).OrderByDescending(x=>x.CreateDate).ToList();
        }
        public void InsertPracticeInfo(PracticeInfo data)
        {
            data.CreateUser = "";
            data.CreateDate = DateTime.Now;
            Data.PracticeInfo.Insert_PracticeInfo(data, null, new string[] { "ID" });
          
        }
        public string InsertUserDetail(List<ExamUserDetailInfo> data,string username)
        {
            try
            {
                var Result = "";
                var ListExamUserDetailInfos = new List<ExamUserDetailInfo>();
                foreach (var item in data)
                {
                    item.DirectorCreateDate = DateTime.Now;
                    item.DirectorCreateUser = username;
                    item.RuleName = item.RuleName;
                    Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(item, null, new string[] { "ID" });

                };
                GetAllExamUserDetailInfo();
                return Result;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void SerachDetailByUserCode(string Code)
        {
            LExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = Code });
            
        }
        public void Stopuser(string ID,string username)
        {
            try
            {
                var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { ID = ID });
                c.IsStop = true;
                c.HrCheckCreateDate = DateTime.Now;
                c.HrCheckCreateUser = username;
                Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(c, null, new string[] { "ID" });
                LSignedupUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup", IsStop = false, DepartCode = "KQ12" });
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }
    }
}