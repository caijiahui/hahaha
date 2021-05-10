using System;

namespace advt.Entity
{
    public partial class ExamRole
    {
        #region ExamRole , (Ver:2.3.8) at: 2021/5/10 16:19:10

        public int ID { get; set; }

        public string RoleName { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
        #endregion
    }
}