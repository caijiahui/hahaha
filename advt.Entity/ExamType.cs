using System;

namespace advt.Entity
{
    public partial class ExamType
    {

        public int ID { get; set; }

        public string TypeName { get; set; }//������������

        public string SuperAdmin { get; set; }//��������Ա

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}