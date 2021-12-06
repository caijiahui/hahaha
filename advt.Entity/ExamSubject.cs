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
        public bool IsSysPractice { get; set; }//实践成绩是否自动从系统串入
        public bool IsProAssess { get; set; }//是否跨制程考核
        public string  DepartCode { get; set; }

    }
}