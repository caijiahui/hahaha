using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class  U_Cancel_UserInfo_History
    {
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string DepartCode { get; set; }
        public DateTime? ExamDate { get; set; }
        public string SubjectName { get; set; }
        public string PostID { get; set; }
        public string STATE { get; set; }
        public DateTime? Pre_InsertDate { get; set; }
        public DateTime? Operate_date { get; set; }
        public string Operate_User { get; set; }
        public int ElectronicQuota { get; set; }//岗位津贴       
        public int MajorQuota { get; set; }//专业加给
        public int SkillsAllowance { get; set; }//技能津贴
        public int GradePosition { get; set; }//职等晋升
        public int PostQuota { get; set; }//岗位等级
        public int TotalQuota { get; set; }//汇总津贴

    }
}
