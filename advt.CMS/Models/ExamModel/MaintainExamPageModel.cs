
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using advt.Entity;

namespace advt.CMS.Models.ExamModel
{
    public class MaintainExamPageModel
    {
        public PageInfo Model { get; set; }
        public List<PageInfo> ListPageInfo { get; set; }
        public List<ExamUserDetailInfo> ListExamUserDetailInfo { get; set; }
        public MaintainExamPageModel() : base()
        {
            Model = new PageInfo();
            ListPageInfo = new List<PageInfo>();
            ListExamUserDetailInfo = new List<ExamUserDetailInfo>();
        }
        public void GetPageInfo(string UserCode ="" ,string SubjectName ="",string ExamDate="")
        {
            if (!string.IsNullOrEmpty(UserCode) && !string.IsNullOrEmpty(SubjectName))
            {
                ListExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = UserCode, SubjectName = SubjectName });
            }
            else if(!string.IsNullOrEmpty(UserCode))
            {
                ListExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = UserCode });
            }
            else if (!string.IsNullOrEmpty(SubjectName))
            {
                ListExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { SubjectName = SubjectName });
            }
            else if (!string.IsNullOrEmpty(ExamDate))
            {
                DateTime date = Convert.ToDateTime(ExamDate);
                ListExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserInfo(date);
            }
             if (string.IsNullOrEmpty(UserCode) &&string.IsNullOrEmpty(SubjectName)&&string.IsNullOrEmpty(ExamDate))
            {
                ListExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo();
            }

            if (ListExamUserDetailInfo.Count() > 0 && ListExamUserDetailInfo != null)
            {
                foreach (var item in ListExamUserDetailInfo)
                {
                    //实践成绩

                    ////申请等级
                    //var SkillName = "";
                    ////岗位津贴
                    //var Allowance = Data.SkillInfo.Get_All_SkillInfo(new { SkillName = SkillName });

                    ////技能等级通过后可晋升的新职等
                    //var NewRankName = "";
                    
                    ////晋升加给
                    //var PromotionBonus = "";
                    //var Bonus = Data.RankInfo.get(new { SkillName = SkillName });

                    ListPageInfo.Add(new PageInfo
                    {
                        UserCode = item.UserCode,
                        UserName = item.UserName,
                        DepartCode = item.DepartCode,
                        PostName = item.PostName,
                        RankName = item.RankName,
                        EntryDate = item.EntryDate,
                        OrganizingFunction = "",
                        CurrentLevel = item.SkillName,//本职等技能
                        ApplyLevel = item.ApplyLevel,//目前技能等级
                        CurrectExamDate = item.UserExamDate,//最近一次考试时间
                         //PostState="在职",
                         //PostJob="BPE",
                        SubjectName = item.SubjectName,
                        ExamScore = item.ExamScore,//最近一次理论成绩
                        PracticeScore = item.PracticeScore,
                        PracticeDate = DateTime.Now,//最近一次实践成绩通过时间
                        FullScale = "是"
                    });
                }
            }


        }
    }
    public class PageInfo
    {
        public string UserCode { get; set; }//工号
        public string UserName { get; set; }//姓名
        public string DepartCode { get; set; }//部门
        public string PostName { get; set; }//职称
        public string RankName { get; set; }//职等
        public DateTime? EntryDate { get; set; }//入职日期
        public string PostState { get; set; }//在职状态
        public string PostJob { get; set; }//本职岗位
        public string SubjectName { get; set; }//考核岗位
        public string SkillName { get; set; } //申请等级
        public decimal? ExamScore { get; set; }//理论考核
        public decimal? PracticeScore { get; set; }//实践考核
        public int SkillAllowance { get; set; }//标准等级津贴
        public string NewRankName { get; set; }//技能等级通过后可晋升的新职等
        public int PromotionBonus { get; set; }//晋升加给
        public int ToatlBonus { get; set; } //此次上调金额
        public string  CurrentMonth { get; set; }//薪资调整月份
        public string CurrentLevel { get; set; }  //本职等技能
        public string ApplyLevel { get; set; }  //目前技能等级
        public DateTime? CurrectExamDate { get; set; }  //最近一次考试时间
        public DateTime? PracticeDate { get; set; }  //最近一次实践成绩通过时间
        public string FullScale { get; set; }//是否满级
        public string OrganizingFunction { get; set; }//组织职能性质
    }
}