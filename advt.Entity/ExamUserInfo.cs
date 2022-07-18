using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class ExamUserInfo
    {
        #region ExamUserInfo , (Ver:2.3.8) at: 2021/2/5 11:13:24

        public int ID { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string DepartCode { get; set; }

        public string PostName { get; set; }

        public string RankName { get; set; }

        public DateTime? EntryDate { get; set; }

        public string Achievement { get; set; }

        public string TypeName { get; set; }

        public string SubjectName { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ReverseBuckle { get; set; }//本职等对应技能等级
        public DateTime? ReverseBuckleDate { get; set; }//绩效可扣时间
        public string ReverseBuckleUser { get; set; }//绩效可扣时间
        public string ApplicationLevel { get; set; }
        public string PostID { get; set; }//岗位编号
        public string WorkPlace { get; set; }//区域
        public bool isJobStatus { get; set; }//区域

        public decimal? Quota { get; set; }//津贴

        public string StopUser { get; set; } //停止人员
        public DateTime? StopDate { get; set; } //停止时间

        public bool EStatus { get; set; } //电子端考试通过后变为true

        public bool IsEnable { get; set; }//电子岗位点击删除后变为true
        public string LocalGrade { get; set; }//等级：初级、高级
        #endregion
    }
}
