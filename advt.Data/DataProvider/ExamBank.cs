using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamBank
    {

        #region ExamBank , (Ver:2.3.8) at: 2021/1/12 15:44:47

        public static List<Entity.ExamBank> Get_All_ExamBank(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamBank(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamBank>(reader);
        }

        public static List<Entity.ExamBank> Get_All_ExamBank()
        {
            return Get_All_ExamBank(null);
        }
        public static List<Entity.ExamBank> Get_All_ExamBank_ExamType(string ExamType)
        {
            return Get_All_ExamBank(new { ExamType = ExamType });
        }
        public static List<Entity.ExamBank> Get_All_ExamBank_ExamType_Rule(string TopicType,string TopicMajor,string TopicLevel,string ExamSubject)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamBank_ExamType_Rule(TopicType,TopicMajor,TopicLevel, ExamSubject);
            return SqlHelper.GetReaderToList<Entity.ExamBank>(reader);
        }

        public static Entity.ExamBank Get_ExamBank(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamBank(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamBank>(reader);
        }

        public static Entity.ExamBank Get_ExamBank(int ID)
        {
            return Get_ExamBank(new { ID = ID });
        }


        public static int Insert_ExamBank(Entity.ExamBank info)
        {
            return Insert_ExamBank(info, null, null);
        }

        public static int Insert_ExamBank(Entity.ExamBank info, string[] Include)
        {
            return Insert_ExamBank(info, Include, null);
        }

        public static int Insert_ExamBank(Entity.ExamBank info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamBank(info, Include, Exclude);
        }

        public static int Update_ExamBank(Entity.ExamBank info)
        {
            return Update_ExamBank(info, null, null);
        }

        public static int Update_ExamBank(Entity.ExamBank info, string[] Include)
        {
            return Update_ExamBank(info, Include, null);
        }

        public static int Update_ExamBank(Entity.ExamBank info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamBank(info, Include, Exclude);
        }

        public static int Delete_ExamBank(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamBank(ID);
        }
        #endregion
    }
}