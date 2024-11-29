using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class ExamUserDetailInfo
    {
        #region ExamUserDetailInfo , (Ver:2.3.8) at: 2021/2/5 11:28:28

        public int ID { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string DepartCode { get; set; }

        public string PostName { get; set; }

        public string RankName { get; set; }

        public string SkillName { get; set; }

        public DateTime? EntryDate { get; set; }

        public string Achievement { get; set; }

        public DateTime? ExamDate { get; set; }

        public decimal? ExamScore { get; set; }

        public decimal? PracticeScore { get; set; }

        public DateTime? PlanExamDate { get; set; }

        public string ExamPlace { get; set; }

        public string ExamStatus { get; set; }

        public bool? IsReview { get; set; }

        public string RuleName { get; set; }

        public string SubjectName { get; set; }

        public string TypeName { get; set; }
        public string ApplyLevel { get; set; }
        public string HighestLevel { get; set; }
        public string IsAchievement { get; set; }
        public string IsExam { get; set; }//是否可考
        public bool IsStop { get; set; }
        public string HrCreateUser { get; set; }
        public DateTime? HrCreateDate { get; set; }
        public string DirectorCreateUser { get; set; }
        public DateTime? DirectorCreateDate { get; set; }
        public string HrCheckCreateUser { get; set; }
        public DateTime? HrCheckCreateDate { get; set; }
        public string StopCreateUser { get; set; }
        public DateTime? StopCreateDate { get; set; }
        public DateTime? UserExamDate { get; set; }
        public string IsUserExam { get; set; }
        public bool ExamStatue { get; set; }
        public bool IsExamPass { get; set; }
        public string WorkPlace { get; set; }
        public string PostID { get; set; }
        public string OrgName { get; set; }//区域名
        public string SignType { get; set; }//报名类型
        #endregion


        public int ElectronicQuota { get; set; }//岗位津贴       
        public int MajorQuota { get; set; }//专业加给
        public int SkillsAllowance { get; set; }//技能津贴
        public int GradePosition { get; set; }//职等晋升
        public int PostQuota { get; set; }//岗位等级
        public int TotalQuota { get; set; }//汇总津贴
        public string State { get; set; }
        public string Type { get; set; }//考试类别：区别chassis人员考试，恢复考，全员复考，降级考
        //点名机制
        public bool IsStartExam { get; set; }//是否启动考试
        public string StartExamUser { get; set; }//点名人
        public DateTime? StartExamDate { get; set; }//点名时间

        public string ExamKind { get; set; }
        public string DutyType { get; set; }//班别

    }
}
