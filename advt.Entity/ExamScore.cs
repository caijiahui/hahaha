using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class ExamScore
    {
        #region ExamScore , (Ver:2.3.8) at: 2021/1/30 14:23:24

        public int ExamID { get; set; }

        public string ExamType { get; set; }

        public bool? IsTest { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CorrectNum { get; set; }
        public  decimal? CorrectScore { get; set; }
        public decimal?  TotalScore { get; set; }
        #endregion
    }
}
