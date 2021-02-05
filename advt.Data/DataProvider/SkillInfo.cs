using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class SkillInfo
    {
        #region SkillInfo , (Ver:2.3.8) at: 2021/2/5 11:37:14

        public static List<Entity.SkillInfo> Get_All_SkillInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_SkillInfo(objparams);
            return SqlHelper.GetReaderToList<Entity.SkillInfo>(reader);
        }

        public static List<Entity.SkillInfo> Get_All_SkillInfo()
        {
            return Get_All_SkillInfo(null);
        }

        public static Entity.SkillInfo Get_SkillInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_SkillInfo(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.SkillInfo>(reader);
        }

        public static Entity.SkillInfo Get_SkillInfo(int ID)
        {
            return Get_SkillInfo(new { ID = ID });
        }

        public static int Insert_SkillInfo(Entity.SkillInfo info)
        {
            return Insert_SkillInfo(info, null, null);
        }

        public static int Insert_SkillInfo(Entity.SkillInfo info, string[] Include)
        {
            return Insert_SkillInfo(info, Include, null);
        }

        public static int Insert_SkillInfo(Entity.SkillInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_SkillInfo(info, Include, Exclude);
        }

        public static int Update_SkillInfo(Entity.SkillInfo info)
        {
            return Update_SkillInfo(info, null, null);
        }

        public static int Update_SkillInfo(Entity.SkillInfo info, string[] Include)
        {
            return Update_SkillInfo(info, Include, null);
        }

        public static int Update_SkillInfo(Entity.SkillInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_SkillInfo(info, Include, Exclude);
        }

        public static int Delete_SkillInfo(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_SkillInfo(ID);
        }
        #endregion
    }
}