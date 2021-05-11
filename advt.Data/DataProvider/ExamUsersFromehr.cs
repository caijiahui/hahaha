using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamUsersFromehr
    {

        #region ExamUsersFromehr , (Ver:2.3.8) at: 2021/5/11 11:39:36

        public static List<Entity.ExamUsersFromehr> Get_All_ExamUsersFromehr(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUsersFromehr(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamUsersFromehr>(reader);
        }

        public static List<Entity.ExamUsersFromehr> Get_All_ExamUsersFromehr()
        {
            return Get_All_ExamUsersFromehr(null);
        }

        public static Entity.ExamUsersFromehr Get_ExamUsersFromehr(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUsersFromehr(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamUsersFromehr>(reader);
        }

        public static Entity.ExamUsersFromehr Get_ExamUsersFromehr(string UserCode)
        {
            return Get_ExamUsersFromehr(new { UserCode = UserCode });
        }

        public static int Insert_ExamUsersFromehr(Entity.ExamUsersFromehr info)
        {
            return Insert_ExamUsersFromehr(info, null, null);
        }

        public static int Insert_ExamUsersFromehr(Entity.ExamUsersFromehr info, string[] Include)
        {
            return Insert_ExamUsersFromehr(info, Include, null);
        }

        public static int Insert_ExamUsersFromehr(Entity.ExamUsersFromehr info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamUsersFromehr(info, Include, Exclude);
        }

        public static int Update_ExamUsersFromehr(Entity.ExamUsersFromehr info)
        {
            return Update_ExamUsersFromehr(info, null, null);
        }

        public static int Update_ExamUsersFromehr(Entity.ExamUsersFromehr info, string[] Include)
        {
            return Update_ExamUsersFromehr(info, Include, null);
        }

        public static int Update_ExamUsersFromehr(Entity.ExamUsersFromehr info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamUsersFromehr(info, Include, Exclude);
        }

        public static int Delete_ExamUsersFromehr(string UserCode)
        {
            return DatabaseProvider.GetInstance().Delete_ExamUsersFromehr(UserCode);
        }
        #endregion
    }
}