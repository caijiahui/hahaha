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
        public string PointScore { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
    }
}
