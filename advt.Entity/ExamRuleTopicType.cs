using System;

namespace advt.Entity
{
    public partial class ExamRuleTopicType
    {

        #region ExamRuleTopicType , (Ver:2.3.8) at: 2021/1/12 15:49:07

        public int ID { get; set; }

        public string TopicMajor { get; set; }

        public string TopicLevel { get; set; }

        public string TopicType { get; set; }

        public string TopicNum { get; set; }

        public bool? TopicScore { get; set; }

        public int? RuleId { get; set; }
        #endregion
    }
}