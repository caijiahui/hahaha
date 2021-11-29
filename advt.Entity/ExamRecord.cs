using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity
{
    public partial class ExamRecord
    {
        #region ExamRecord , (Ver:2.3.8) at: 2021/1/30 14:28:04

        public int ID { get; set; }

        public string ExamID { get; set; }

        public string TopicTitle { get; set; }

        public string TopicTitlePicNum { get; set; }

        public string OptionA { get; set; }
      

        public string OptionAPicNum { get; set; }

        public string OptionB { get; set; }
     

        public string OptionBPicNum { get; set; }

        public string OptionC { get; set; }
       
        public string OptionCPicNum { get; set; }

        public string OptionD { get; set; }
     
        public string OptionDPicNum { get; set; }

        public string OptionE { get; set; }
      
        public string OptionEPicNum { get; set; }

        public string OptionF { get; set; }
      
        public string OptionFPicNum { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateUser { get; set; }
        public decimal TopicNum { get; set; }
        //正确答案
        public string  CorrectAnsower { get; set; }
        //选择答案
        public string WriteAnsower { get; set; }
        public string Type { get; set; }

        public string Remark { get; set; }
        public string DaRemark { get; set; }
        public bool IsRight { get; set; }
        public string ExamGuid { get; set; }
        #endregion
    }
}
