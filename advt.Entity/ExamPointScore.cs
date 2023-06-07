using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class ExamPointScore
    {
        public int ID { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }
        public int PointScore { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
