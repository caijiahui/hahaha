using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region ExamRule , (Ver:2.3.8) at: 2021/1/7 16:05:32

        IDataReader Get_All_ExamRule(object objparams);

        int Insert_ExamRule(Entity.ExamRule info, string[] Include, string[] Exclude);

        int Update_ExamRule(Entity.ExamRule info, string[] Include, string[] Exclude);

        int Delete_ExamRule(int ID);

        IDataReader GetSubjectList(string model);
        IDataReader GetTopicInfo(string model);
        IDataReader GetTRuleSubjectInfo(string model);
        IDataReader Get_All_ExamRuleInfo(string RuleName);
        IDataReader Get_All_ExamGetRuleName(string RuleName);
        
        #endregion
    }
}
