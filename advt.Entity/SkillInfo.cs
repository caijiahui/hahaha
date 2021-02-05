using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class SkillInfo
    {
        #region SkillInfo , (Ver:2.3.8) at: 2021/2/5 11:37:14

        public int ID { get; set; }

        public string SKillName { get; set; }

        public string AchRequire { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
        #endregion
    }
}
