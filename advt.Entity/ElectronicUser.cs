using System;

namespace advt.Entity
{
    public partial class ElectronicUser
    {
        #region ElectronicUser , (Ver:2.3.8) at: 2022/1/27 15:46:50

        public string UserCode { get; set; }

        public string ElectronicPost { get; set; }

        public DateTime? CreateDate { get; set; }

        public decimal? ElectronicQuota { get; set; }

        public bool IsEnable { get; set; }

        public int ID { get; set; }

        public string UserName { get; set; }

        public string Department { get; set; }
        public string SubjectName { get; set; }
        public string StopUser { get; set; }
        public DateTime? StopDate { get; set; }
        public string CreateUser { get; set; }
        #endregion
    }
}