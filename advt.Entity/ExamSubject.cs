using System;

namespace advt.Entity
{
    public partial class ExamSubject
    {

        public int ID { get; set; }

        public string SubjectName { get; set; }//考试类型名称
        public string TypeName { get; set; }//类型ID
        public string ExamRuleName { get; set; }//考试规则
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int HCLimit { get; set; }//HC限制
        public int ElectronicQuota { get; set; }//岗位津贴
        public bool IsSysPractice { get; set; }//实践成绩是否自动从系统串入
        public bool IsProAssess { get; set; }//是否跨制程考核
        public string  DepartCode { get; set; }
        public int MajorQuota { get; set; }//专业加给
        public int SkillsAllowance { get; set; }//技能津贴
        public int GradePosition { get; set; }//职等晋升
        public bool IsAddAllowance { get; set; }//津贴累积计算/津贴增加计算
        public int PostQuota { get; set; }//岗位等级

    }
}