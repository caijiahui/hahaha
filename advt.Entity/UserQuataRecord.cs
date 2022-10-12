using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class UserQuataRecord
    {
        public int ID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string TypeName { get; set; }
        public string SubjectName { get; set; }
        public string RuleName { get; set; }
        public string CreateName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int ElectronicQuota { get; set; }//岗位津贴       
        public int MajorQuota { get; set; }//专业加给
        public int SkillsAllowance { get; set; }//技能津贴
        public int GradePosition { get; set; }//职等晋升
        public int PostQuota { get; set; }//岗位等级
        public int TotalQuota { get; set; }//汇总津贴
    }
}
