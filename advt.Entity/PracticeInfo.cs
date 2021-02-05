using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class PracticeInfo
    {
        #region PracticeInfo , (Ver:2.3.8) at: 2021/2/5 11:41:00

        public int ID { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public DateTime? ValidityDate { get; set; }

        public decimal? PracticeScore { get; set; }

        public string PracticeRemark { get; set; }

        public string Enclosure { get; set; }

        public string SkillName { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
        #endregion
    }
}
