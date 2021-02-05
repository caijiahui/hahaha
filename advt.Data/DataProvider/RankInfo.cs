using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class RankInfo
    {

        #region RankInfo , (Ver:2.3.8) at: 2021/2/5 11:33:07

        public static List<Entity.RankInfo> Get_All_RankInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_RankInfo(objparams);
            return SqlHelper.GetReaderToList<Entity.RankInfo>(reader);
        }

        public static List<Entity.RankInfo> Get_All_RankInfo()
        {
            return Get_All_RankInfo(null);
        }

        public static Entity.RankInfo Get_RankInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_RankInfo(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.RankInfo>(reader);
        }

        public static Entity.RankInfo Get_RankInfo(int ID)
        {
            return Get_RankInfo(new { ID = ID });
        }

        public static int Insert_RankInfo(Entity.RankInfo info)
        {
            return Insert_RankInfo(info, null, null);
        }

        public static int Insert_RankInfo(Entity.RankInfo info, string[] Include)
        {
            return Insert_RankInfo(info, Include, null);
        }

        public static int Insert_RankInfo(Entity.RankInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_RankInfo(info, Include, Exclude);
        }

        public static int Update_RankInfo(Entity.RankInfo info)
        {
            return Update_RankInfo(info, null, null);
        }

        public static int Update_RankInfo(Entity.RankInfo info, string[] Include)
        {
            return Update_RankInfo(info, Include, null);
        }

        public static int Update_RankInfo(Entity.RankInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_RankInfo(info, Include, Exclude);
        }

        public static int Delete_RankInfo(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_RankInfo(ID);
        }
        #endregion
    }
}