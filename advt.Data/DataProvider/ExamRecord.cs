using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamRecord
    {

        #region ExamRecord , (Ver:2.3.8) at: 2021/1/30 14:28:04

        public static List<Entity.ExamRecord> Get_All_ExamRecord(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRecord(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamRecord>(reader);
        }

        public static List<Entity.ExamRecord> Get_All_ExamRecord()
        {
            return Get_All_ExamRecord(null);
        }

        public static Entity.ExamRecord Get_ExamRecord(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRecord(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamRecord>(reader);
        }

        public static Entity.ExamRecord Get_ExamRecord(int ID)
        {
            return Get_ExamRecord(new { ID = ID });
        }

        public static int Insert_ExamRecord(Entity.ExamRecord info)
        {
            return Insert_ExamRecord(info, null, null);
        }

        public static int Insert_ExamRecord(Entity.ExamRecord info, string[] Include)
        {
            return Insert_ExamRecord(info, Include, null);
        }

        public static int Insert_ExamRecord(Entity.ExamRecord info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamRecord(info, Include, Exclude);
        }

        public static int Update_ExamRecord(Entity.ExamRecord info)
        {
            return Update_ExamRecord(info, null, null);
        }

        public static int Update_ExamRecord(Entity.ExamRecord info, string[] Include)
        {
            return Update_ExamRecord(info, Include, null);
        }

        public static int Update_ExamRecord(Entity.ExamRecord info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamRecord(info, Include, Exclude);
        }

        public static int Delete_ExamRecord(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamRecord(ID);
        }
        #endregion
    }
}