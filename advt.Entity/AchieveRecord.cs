using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class AchieveRecord
    {
        #region AchieveRecord 
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string Achievement { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string RecordType { get; set; }
        #endregion
    }
}
