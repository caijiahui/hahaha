using System;

namespace advt.Entity
{
    public partial class ExamBank
    {

        #region ExamBank , (Ver:2.3.8) at: 2021/1/12 15:44:47

        public int ID { get; set; }

        public string ExamType { get; set; }

        public string ExamSubject { get; set; }

        public string TopicMajor { get; set; }//C#

        public string TopicLevel { get; set; }//G1

        public string TopicType { get; set; }//ÎÊ´ð

        public string TopicTitle { get; set; }

        public string TopicTitlePicNum { get; set; }

        public string RightKey { get; set; }

        public string Remark { get; set; }

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
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public int Bcount { get; set; }
        #endregion
    }
}