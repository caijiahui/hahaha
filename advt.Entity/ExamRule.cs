using System;

namespace advt.Entity
{
    public partial class ExamRule
    {

        #region ExamRule , (Ver:2.3.8) at: 2021/1/9 12:16:03

        public int ID { get; set; }
        public string RuleName { get; set; }//规则名
        public string TypeName { get; set; }//考试类型
        public string SubjectName { get; set; }//考试科目

        public decimal TotalScore { get; set; }//总分

        public decimal TotalTime { get; set; }//总时长

        public int TotalSubject { get; set; }//总题

        public decimal PassScore { get; set; }//通过分数

        public bool IsRead { get; set; }

        public bool IsRepeat { get; set; }

        public string StartDeac { get; set; }

        public string EndDesc { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool IsQuestion { get; set; }
        public decimal PassPracticeScore { get; set; }
        #endregion
    }
}