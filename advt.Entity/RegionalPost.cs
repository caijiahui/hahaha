using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class RegionalPost
    {
        public int ID { get; set; }
        public string RegionalPlace { get; set; }//涉及区域
        public string DepartCode { get; set; }//部门代码
        public string PostID { get; set; }//岗位编号
        public string PostName { get; set; }//岗位-工种
        public string RuleName { get; set; }//岗位-工种
        public string RuleTwoName { get; set; }//岗位2-工种
        public string ExamType { get; set; }
        public DateTime CreateDate { get; set; }
        public string  CreateUser { get; set; }
        public string PostType { get; set; }//Chassis岗位类型：一般岗、技术岗
        public string PostCycle { get; set; }//是否固定考试月份
        public bool IsPostCycle { get; set; }
        public bool IsWorkState { get; set; }//是否“转正”才可考试
        public string ExamEntry { get; set; }//是否要求入职时间,多久考试
        public string ExamEntryNum { get; set; }
        public string RegionRank { get; set; }//职等下拉“A职等”
        public bool IsExamEntry { get; set; }//是否要
        public bool IsEveryExam { get; set; }//是否每月考试
        public bool IsRuleOneShangGang { get; set; }//是否判断规则上岗证
        public bool IsRuleTwoShangGang { get; set; }//是否判断规则上岗证
        public bool IsRuleOneAch { get; set; }//是否判断绩效成绩
        public bool IsRuleTwoAch { get; set; }//是否判断绩效成绩
        public bool IsRuleOneQuality { get; set; }//是否判断品质扣分
        public bool IsRuleTwoQuality { get; set; }//是否判断品质扣分
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
