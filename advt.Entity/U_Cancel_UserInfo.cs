using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class U_Cancel_UserInfo
    {
        public int ID { get; set; }
        public int HC { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string DepartCode { get; set; }
        public DateTime? ExamDate { get; set; }
        public string SubjectName { get; set; }
        public string PostID { get; set; }
        public string STATE { get; set; }
        public DateTime? InsertDate { get; set; }

    }
}
