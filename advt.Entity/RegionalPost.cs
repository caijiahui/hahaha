using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class RegionalPost
    {
        public int ID { get; set; }
        public string RegionalPlace { get; set; }//涉及区域
        public string DepartCode { get; set; }//部门代码
        public string PostID { get; set; }//岗位编号
        public string PostName { get; set; }//岗位-工种
        public string RuleName { get; set; }//岗位-工种
        public string RuleTwoName { get; set; }//岗位2-工种
        public string ExamType { get; set; }
        public DateTime CreateDate { get; set; }
        public string  CreateUser { get; set; }
    }
}
