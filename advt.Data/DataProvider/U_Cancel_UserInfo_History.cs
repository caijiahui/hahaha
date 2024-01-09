using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class U_Cancel_UserInfo_History
    {
        #region U_Cancel_UserInfo_History , (Ver:2.3.8) at: 2021/2/5 11:37:14

        public static List<Entity.U_Cancel_UserInfo_History> Get_All_U_Cancel_UserInfo_History(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_U_Cancel_UserInfo_History(objparams);
            return SqlHelper.GetReaderToList<Entity.U_Cancel_UserInfo_History>(reader);
        }

        public static List<Entity.U_Cancel_UserInfo_History> Get_All_U_Cancel_UserInfo_History()
        {
            return Get_All_U_Cancel_UserInfo_History(null);
        }

        public static Entity.U_Cancel_UserInfo_History Get_U_Cancel_UserInfo_History(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_U_Cancel_UserInfo_History(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.U_Cancel_UserInfo_History>(reader);
        }

        public static Entity.U_Cancel_UserInfo_History Get_U_Cancel_UserInfo_History(int ID)
        {
            return Get_U_Cancel_UserInfo_History(new { ID = ID });
        }

        public static int Insert_U_Cancel_UserInfo_History(Entity.U_Cancel_UserInfo_History info)
        {
            return Insert_U_Cancel_UserInfo_History(info, null, null);
        }

        public static int Insert_U_Cancel_UserInfo_History(Entity.U_Cancel_UserInfo_History info, string[] Include)
        {
            return Insert_U_Cancel_UserInfo_History(info, Include, null);
        }

        public static int Insert_U_Cancel_UserInfo_History(Entity.U_Cancel_UserInfo_History info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_U_Cancel_UserInfo_History(info, Include, Exclude);
        }

        public static int Update_U_Cancel_UserInfo_History(Entity.U_Cancel_UserInfo_History info)
        {
            return Update_U_Cancel_UserInfo_History(info, null, null);
        }

        public static int Update_U_Cancel_UserInfo_History(Entity.U_Cancel_UserInfo_History info, string[] Include)
        {
            return Update_U_Cancel_UserInfo_History(info, Include, null);
        }

        public static int Update_U_Cancel_UserInfo_History(Entity.U_Cancel_UserInfo_History info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_U_Cancel_UserInfo_History(info, Include, Exclude);
        }

        public static int Delete_U_Cancel_UserInfo_History(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_U_Cancel_UserInfo_History(ID);
        }
        #endregion
    }
}