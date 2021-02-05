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

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
        #endregion
    }
}
