using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class PracticeInfo
    {
        #region PracticeInfo , (Ver:2.3.8) at: 2021/2/5 11:41:00

        public static List<Entity.PracticeInfo> Get_All_PracticeInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_PracticeInfo(objparams);
            return SqlHelper.GetReaderToList<Entity.PracticeInfo>(reader);
        }

        public static List<Entity.PracticeInfo> Get_All_PracticeInfo()
        {
            return Get_All_PracticeInfo(null);
        }
        public static List<Entity.PracticeInfo> Get__All_PracticeInfo_UserCode(string UserCode, string SkillName)
        {
            return Get_All_PracticeInfo(new { UserCode = UserCode, SkillName = SkillName });
        }
        public static Entity.PracticeInfo Get_PracticeInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_PracticeInfo(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.PracticeInfo>(reader);
        }


        public static Entity.PracticeInfo Get_PracticeInfo(int ID)
        {
            return Get_PracticeInfo(new { ID = ID });
        }

        public static int Insert_PracticeInfo(Entity.PracticeInfo info)
        {
            return Insert_PracticeInfo(info, null, null);
        }

        public static int Insert_PracticeInfo(Entity.PracticeInfo info, string[] Include)
        {
            return Insert_PracticeInfo(info, Include, null);
        }

        public static int Insert_PracticeInfo(Entity.PracticeInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_PracticeInfo(info, Include, Exclude);
        }

        public static int Update_PracticeInfo(Entity.PracticeInfo info)
        {
            return Update_PracticeInfo(info, null, null);
        }

        public static int Update_PracticeInfo(Entity.PracticeInfo info, string[] Include)
        {
            return Update_PracticeInfo(info, Include, null);
        }

        public static int Update_PracticeInfo(Entity.PracticeInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_PracticeInfo(info, Include, Exclude);
        }

        public static int Delete_PracticeInfo(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_PracticeInfo(ID);
        }
        #endregion
    }
}