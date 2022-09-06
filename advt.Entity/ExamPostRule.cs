using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class ExamPostRule
    {
        public int ID { get; set; }
		public string PostID { get; set; }
		public string PostName { get; set; }
		public string DepartCode { get; set; }
		public string RuleName { get; set; }//规则
		public bool IsRuleAch { get; set; }//是否判断绩效
		public bool IsRuleQuality { get; set; }//是否判断品质
		public bool IsRuleShangGang { get; set; }//是否判断上岗
		public string PostExamEntry { get; set; }//按照入职判断考试
		public string CreateUser { get; set; }
		public DateTime? CreateDate { get; set; }
		public string UpdateUser { get; set; }
		public DateTime? UpdateDate { get; set; }
		
	}
}
