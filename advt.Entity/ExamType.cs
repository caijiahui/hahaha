using System;

namespace advt.Entity
{
    public partial class ExamType
    {

        public int ID { get; set; }

        public string TypeName { get; set; }//考试类型名称

        public string SuperAdmin { get; set; }//超级管理员

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}