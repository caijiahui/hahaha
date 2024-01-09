using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial  class U_Cancel_UserInfo
    {
        public static List<Entity.U_Cancel_UserInfo> Get_All_U_Cancel_UserInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_U_Cancel_UserInfo(objparams);
            return SqlHelper.GetReaderToList<Entity.U_Cancel_UserInfo>(reader);
        }

        public static List<Entity.U_Cancel_UserInfo> Get_All_U_Cancel_UserInfo()
        {
            return Get_All_U_Cancel_UserInfo(null);
        }

        public static Entity.U_Cancel_UserInfo Get_U_Cancel_UserInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_U_Cancel_UserInfo(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.U_Cancel_UserInfo>(reader);
        }

        public static Entity.U_Cancel_UserInfo Get_U_Cancel_UserInfo(int ExamID)
        {
            return Get_U_Cancel_UserInfo(new { ExamID = ExamID });
        }

        public static int Insert_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info)
        {
            return Insert_U_Cancel_UserInfo(info, null, null);
        }

        public static int Insert_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info, string[] Include)
        {
            return Insert_U_Cancel_UserInfo(info, Include, null);
        }

        public static int Insert_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_U_Cancel_UserInfo(info, Include, Exclude);
        }
        public static int Update_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info)
        {
            return Update_U_Cancel_UserInfo(info, null, null);
        }

        public static int Update_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info, string[] Include)
        {
            return Update_U_Cancel_UserInfo(info, Include, null);
        }

        public static int Update_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_U_Cancel_UserInfo(info, Include, Exclude);
        }
        public static List<Entity.U_Cancel_UserInfo> GetAllCancelUserInfo(string SubjectName, string searchdata)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetAllCancelUserInfo(SubjectName, searchdata);
            return SqlHelper.GetReaderToList<Entity.U_Cancel_UserInfo>(reader);
        }
        public static int Delete_U_Cancel_UserInfo(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_U_Cancel_UserInfo(ID);
        }

    }
}
