using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamScore
    {

        #region ExamScore , (Ver:2.3.8) at: 2021/1/30 14:23:24

        public static List<Entity.ExamScore> Get_All_ExamScore(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamScore(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamScore>(reader);
        }

        public static List<Entity.ExamScore> Get_All_ExamScore()
        {
            return Get_All_ExamScore(null);
        }

        public static Entity.ExamScore Get_ExamScore(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamScore(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamScore>(reader);
        }

        public static Entity.ExamScore Get_ExamScore(int ExamID)
        {
            return Get_ExamScore(new { ExamID = ExamID });
        }

        public static int Insert_ExamScore(Entity.ExamScore info)
        {
            return Insert_ExamScore(info, null, null);
        }

        public static int Insert_ExamScore(Entity.ExamScore info, string[] Include)
        {
            return Insert_ExamScore(info, Include, null);
        }

        public static int Insert_ExamScore(Entity.ExamScore info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamScore(info, Include, Exclude);
        }

        public static int Update_ExamScore(Entity.ExamScore info)
        {
            return Update_ExamScore(info, null, null);
        }

        public static int Update_ExamScore(Entity.ExamScore info, string[] Include)
        {
            return Update_ExamScore(info, Include, null);
        }

        public static int Update_ExamScore(Entity.ExamScore info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamScore(info, Include, Exclude);
        }

        
        #endregion
    }
}