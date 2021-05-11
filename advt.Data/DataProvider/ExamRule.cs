using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamRule
    {

        #region ExamSubject , (Ver:2.3.8) at: 2021/1/7 16:05:32

        public static List<Entity.ExamRule> Get_All_ExamRule(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRule(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamRule>(reader);
        }

        public static List<Entity.ExamRule> Get_All_ExamRule()
        {
            return Get_All_ExamRule(null);
        }
        public static List<Entity.ExamRule> Get_All_TypeNameExamRule(string TypeName)
        {
            return Get_All_ExamRule(new { TypeName= TypeName });
        }

        public static Entity.ExamRule Get_ExamRule(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRule(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamRule>(reader);
        }

        public static Entity.ExamRule Get_ExamRule(int ID)
        {
            return Get_ExamRule(new { ID = ID });
        }

        public static int Insert_ExamRule(Entity.ExamRule info)
        {
            return Insert_ExamRule(info, null, null);
        }

        public static int Insert_ExamRule(Entity.ExamRule info, string[] Include)
        {
            return Insert_ExamRule(info, Include, null);
        }

        public static int Insert_ExamRule(Entity.ExamRule info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamRule(info, Include, Exclude);
        }

        public static int Update_ExamRule(Entity.ExamRule info)
        {
            return Update_ExamRule(info, null, null);
        }

        public static int Update_ExamRule(Entity.ExamRule info, string[] Include)
        {
            return Update_ExamRule(info, Include, null);
        }

        public static int Update_ExamRule(Entity.ExamRule info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamRule(info, Include, Exclude);
        }

        public static int Delete_ExamRule(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamRule(ID);
        }
        public static List<Entity.ExamSubject> GetSubjectList()
        {
            return GetSubjectList(null);
        }
        public static List<Entity.ExamSubject> GetSubjectList(string model)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetSubjectList(model);
            return SqlHelper.GetReaderToList<Entity.ExamSubject>(reader);
        }
        public static List<Entity.ExamBank> GetTopicInfo()
        {
            return GetTopicInfo(null);
        }
        public static List<Entity.ExamBank> GetTopicInfo(string model)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicInfo(model);
            return SqlHelper.GetReaderToList<Entity.ExamBank>(reader);
        }
        public static List<Entity.ExamBank> GetTRuleSubjectInfo()
        {
            return GetTRuleSubjectInfo(null);
        }
        public static List<Entity.ExamBank> GetTRuleSubjectInfo(string model)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTRuleSubjectInfo(model);
            return SqlHelper.GetReaderToList<Entity.ExamBank>(reader);
        }
        public static List<Entity.ExamRule> Get_All_ExamRuleInfo(string RuleName)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRuleInfo(RuleName);
            return SqlHelper.GetReaderToList<Entity.ExamRule>(reader);
        }
        public static List<Entity.ExamRule> Get_All_ExamGetRuleName(string RuleName)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamGetRuleName(RuleName);
            return SqlHelper.GetReaderToList<Entity.ExamRule>(reader);
        }
        

        #endregion
    }
}