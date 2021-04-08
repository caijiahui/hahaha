using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region ExamRuleTopicType , (Ver:2.3.8) at: 2021/1/12 15:49:07

        IDataReader Get_All_ExamRuleTopicType(object objparams);

        int Insert_ExamRuleTopicType(Entity.ExamRuleTopicType info, string[] Include, string[] Exclude);

        int Update_ExamRuleTopicType(Entity.ExamRuleTopicType info, string[] Include, string[] Exclude);

        int Delete_ExamRuleTopicType(int ID);

        IDataReader Get_ExamRuleTopic(string model);
        IDataReader Get_ExamRuleInfo(string TopicLevel, string TopicMajor, string TopicType, int id);

        int DeleteRuleTopicInfo(string TopicMajor, string TopicLevel, string TopicType, int RuleName);
        int DeleteRuleLeTopicInfo(string TopicMajor, string TopicType, int RuleName);
        #endregion
    }
}
