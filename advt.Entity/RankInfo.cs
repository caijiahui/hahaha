using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class RankInfo
    {
        #region RankInfo , (Ver:2.3.8) at: 2021/2/5 11:33:07

        public int ID { get; set; }

        public string RankName { get; set; }

        public string SkillName { get; set; }

        public string MaxSkillName { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string CorrespondingLevel { get; set; }//对应等级可晋升
        public string NewPromotion { get; set; }//新晋升职等
        public int PromotionBonus { get; set; }//晋升加给
        #endregion
    }
}
