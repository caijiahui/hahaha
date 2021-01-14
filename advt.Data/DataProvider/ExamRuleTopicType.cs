using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamRuleTopicType
    {
        #region ExamRuleTopicType , (Ver:2.3.8) at: 2021/1/12 15:49:07

        public static List<Entity.ExamRuleTopicType> Get_All_ExamRuleTopicType(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRuleTopicType(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamRuleTopicType>(reader);
        }

        public static List<Entity.ExamRuleTopicType> Get_All_ExamRuleTopicType()
        {
            return Get_All_ExamRuleTopicType(null);
        }

        public static Entity.ExamRuleTopicType Get_ExamRuleTopicType(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRuleTopicType(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamRuleTopicType>(reader);
        }

        public static Entity.ExamRuleTopicType Get_ExamRuleTopicType(int ID)
        {
            return Get_ExamRuleTopicType(new { ID = ID });
        }

        public static int Insert_ExamRuleTopicType(Entity.ExamRuleTopicType info)
        {
            return Insert_ExamRuleTopicType(info, null, null);
        }

        public static int Insert_ExamRuleTopicType(Entity.ExamRuleTopicType info, string[] Include)
        {
            return Insert_ExamRuleTopicType(info, Include, null);
        }

        public static int Insert_ExamRuleTopicType(Entity.ExamRuleTopicType info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamRuleTopicType(info, Include, Exclude);
        }

        public static int Update_ExamRuleTopicType(Entity.ExamRuleTopicType info)
        {
            return Update_ExamRuleTopicType(info, null, null);
        }

        public static int Update_ExamRuleTopicType(Entity.ExamRuleTopicType info, string[] Include)
        {
            return Update_ExamRuleTopicType(info, Include, null);
        }

        public static int Update_ExamRuleTopicType(Entity.ExamRuleTopicType info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamRuleTopicType(info, Include, Exclude);
        }

        public static int Delete_ExamRuleTopicType(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamRuleTopicType(ID);
        }
        public static List<Entity.ExamRuleTopicType> Get_ExamRuleTopic()
        {
            return Get_ExamRuleTopic(null);
        }
        public static List<Entity.ExamRuleTopicType> Get_ExamRuleTopic(string model)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_ExamRuleTopic(model);
            return SqlHelper.GetReaderToList<Entity.ExamRuleTopicType>(reader);
        }
        public static List<Entity.ExamRuleTopicType> Get_ExamRuleInfo()
        {
            return Get_ExamRuleInfo(null,null,null,0);
        }
        public static List<Entity.ExamRuleTopicType> Get_ExamRuleInfo(string TopicLevel, string TopicMajor, string TopicType,int id)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_ExamRuleInfo(TopicLevel,TopicMajor,TopicType,id);
            return SqlHelper.GetReaderToList<Entity.ExamRuleTopicType>(reader);
        }
        
        #endregion
    }
}