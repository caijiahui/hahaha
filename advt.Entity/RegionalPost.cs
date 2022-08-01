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
        public string PostType { get; set; }//Chassis岗位类型：一般岗、技术岗
        public string PostCycle { get; set; }//Chassis岗位周期
        public bool IsWorkState { get; set; }//是否依据转正
        public string ExamEntry { get; set; }//入职多长时间可以考试
    }
}
