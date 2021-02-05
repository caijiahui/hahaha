using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial class ExamUserDetailInfo
    {
        #region ExamUserDetailInfo , (Ver:2.3.8) at: 2021/2/5 11:28:28

        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserDetailInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserDetailInfo(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }

        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserDetailInfo()
        {
            return Get_All_ExamUserDetailInfo(null);
        }

        public static Entity.ExamUserDetailInfo Get_ExamUserDetailInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserDetailInfo(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamUserDetailInfo>(reader);
        }

        public static Entity.ExamUserDetailInfo Get_ExamUserDetailInfo(int ID)
        {
            return Get_ExamUserDetailInfo(new { ID = ID });
        }

        public static int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info)
        {
            return Insert_ExamUserDetailInfo(info, null, null);
        }

        public static int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include)
        {
            return Insert_ExamUserDetailInfo(info, Include, null);
        }

        public static int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamUserDetailInfo(info, Include, Exclude);
        }

        public static int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info)
        {
            return Update_ExamUserDetailInfo(info, null, null);
        }

        public static int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include)
        {
            return Update_ExamUserDetailInfo(info, Include, null);
        }

        public static int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamUserDetailInfo(info, Include, Exclude);
        }

        public static int Delete_ExamUserDetailInfo(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamUserDetailInfo(ID);
        }
        #endregion
    }
}
