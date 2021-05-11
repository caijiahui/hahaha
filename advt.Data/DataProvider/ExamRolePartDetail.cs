using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamRolePartDetail
    {

        #region ExamRolePartDetail , (Ver:2.3.8) at: 2021/5/11 8:40:48

        public static List<Entity.ExamRolePartDetail> Get_All_ExamRolePartDetail(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRolePartDetail(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamRolePartDetail>(reader);
        }

        public static List<Entity.ExamRolePartDetail> Get_All_ExamRolePartDetail()
        {
            return Get_All_ExamRolePartDetail(null);
        }

        public static Entity.ExamRolePartDetail Get_ExamRolePartDetail(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRolePartDetail(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamRolePartDetail>(reader);
        }

        public static Entity.ExamRolePartDetail Get_ExamRolePartDetail(int ID)
        {
            return Get_ExamRolePartDetail(new { ID = ID });
        }

        public static int Insert_ExamRolePartDetail(Entity.ExamRolePartDetail info)
        {
            return Insert_ExamRolePartDetail(info, null, null);
        }

        public static int Insert_ExamRolePartDetail(Entity.ExamRolePartDetail info, string[] Include)
        {
            return Insert_ExamRolePartDetail(info, Include, null);
        }

        public static int Insert_ExamRolePartDetail(Entity.ExamRolePartDetail info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamRolePartDetail(info, Include, Exclude);
        }

        public static int Update_ExamRolePartDetail(Entity.ExamRolePartDetail info)
        {
            return Update_ExamRolePartDetail(info, null, null);
        }

        public static int Update_ExamRolePartDetail(Entity.ExamRolePartDetail info, string[] Include)
        {
            return Update_ExamRolePartDetail(info, Include, null);
        }

        public static int Update_ExamRolePartDetail(Entity.ExamRolePartDetail info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamRolePartDetail(info, Include, Exclude);
        }

        public static int Delete_ExamRolePartDetail(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamRolePartDetail(ID);
        }
        #endregion
    }
}