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
        public SupervisorAuditModel() : base()
        {
            ListDirectorUserInfos = new List<UserInfo>();
            LRules = new List<ExamRule>();
            LExamUserDetailInfo = new List<ExamUserDetailInfo>();
            LPracticeInfo = new List<PracticeInfo>();
            LSignedupUser = new List<ExamUserDetailInfo>();
        }
        public void GetAllExamUserDetailInfo()
        {
            var model = new ExamUserInfoModel();
            model.GetUserInfo();
            var c = model.ListUserInfo.Where(x => x.DepartCode == "KQ12").ToList();
            //foreach (var item in c)
            //{
            //    decimal PracticalID = 0;
            //    var SkillName = item.SkillLevel;
            //    var Practical = Data.PracticeInfo.Get__All_PracticeInfo_UserCode(item.UserCode, SkillName);
            //    if (Practical.Count()!=0)
            //    {
            //        PracticalID = Convert.ToDecimal(Practical.FirstOrDefault().PracticeScore);
            //    }
            //    //item.ExamStatus = "Signup";
            //    item.PracticalID = PracticalID;
            //}
            ListDirectorUserInfos = c;
            LSignedupUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus= "Signup", IsStop = false, DepartCode= "KQ12" });
            LRules = Data.ExamRule.Get_All_TypeNameExamRule("技能等级考试");
           

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
        public string InsertUserDetail(List<UserInfo> data,string username)
        {
            try
            {
                var Result = "";
                var ListExamUserDetailInfos = new List<ExamUserDetailInfo>();
                foreach (var item in data)
                {
                    var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { TypeName= item.TypeName,  UserCode = item.UserCode, ExamStatus = "Signup", ApplyLevel = item.ApplicationLevel, IsStop=false });
                    if (c != null)
                    {
                        Result += item.UserName + "已报名，不可重复报名";

                    }
                    else
                    {
                        ExamUserDetailInfo v = new ExamUserDetailInfo();
                        v.UserCode = item.UserCode;
                        v.UserName = item.UserName;
                        v.DepartCode = item.DepartCode;
                        v.PostName = item.PostName;
                        v.RankName = item.RankName;
                        v.SkillName = item.SkillLevel; //本职等技能G1
                        v.EntryDate = item.EntryDate; //入职日期
                        v.Achievement = item.Achievement;//绩效
                        v.PracticeScore = item.PracticalID;
                        v.PlanExamDate = item.PlanExamDate;
                        v.ExamStatus = item.ExamStatus;
                        v.IsReview = item.IsReview;
                        v.RuleName = item.RuleName;
                        v.SubjectName = item.SubjectName;
                        v.TypeName = item.TypeName;
                        v.ApplyLevel = item.ApplicationLevel;//本次申请等级满级
                        //v.IsAchievement = item.IsApp;//是否满级
                        v.IsAchievement = item.IsAchment;//是否符合绩效
                        v.HighestLevel = item.HighestTestSkill;//最高可考技能
                        v.IsExam = item.IsExam;
                        v.HrCheckCreateDate = DateTime.Now;
                        v.HrCheckCreateUser = username;
                        Data.ExamUserDetailInfo.Insert_ExamUserDetailInfo(v, null, new string[] { "ID" });
                    }

                };
                LSignedupUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup", IsStop = false, DepartCode = "KQ12" });
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