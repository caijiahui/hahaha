using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial  class AchieveRecord
    {
        #region AchieveRecord , (Ver:2.3.8) at: 2021/2/5 11:41:00

        public static List<Entity.AchieveRecord> Get_All_AchieveRecord(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_AchieveRecord(objparams);
            return SqlHelper.GetReaderToList<Entity.AchieveRecord>(reader);
        }

        public static List<Entity.AchieveRecord> Get_All_AchieveRecord()
        {
            return Get_All_AchieveRecord(null);
        }
        public static List<Entity.AchieveRecord> Get__All_AchieveRecord_UserCode(string UserCode, string SkillName)
        {
            return Get_All_AchieveRecord(new { UserCode = UserCode, SkillName = SkillName });
        }
        public static Entity.AchieveRecord Get_AchieveRecord(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_AchieveRecord(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.AchieveRecord>(reader);
        }


        public static Entity.AchieveRecord Get_AchieveRecord(int ID)
        {
            return Get_AchieveRecord(new { ID = ID });
        }

        public static int Insert_AchieveRecord(Entity.AchieveRecord info)
        {
            return Insert_AchieveRecord(info, null, null);
        }

        public static int Insert_AchieveRecord(Entity.AchieveRecord info, string[] Include)
        {
            return Insert_AchieveRecord(info, Include, null);
        }

        public static int Insert_AchieveRecord(Entity.AchieveRecord info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_AchieveRecord(info, Include, Exclude);
        }

        public static int Update_AchieveRecord(Entity.AchieveRecord info)
        {
            return Update_AchieveRecord(info, null, null);
        }

        public static int Update_AchieveRecord(Entity.AchieveRecord info, string[] Include)
        {
            return Update_AchieveRecord(info, Include, null);
        }

        public static int Update_AchieveRecord(Entity.AchieveRecord info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_AchieveRecord(info, Include, Exclude);
        }

        public static int Delete_AchieveRecord(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_AchieveRecord(ID);
        }
        #endregion
    }
}
