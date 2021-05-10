using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamRole
    {

        #region ExamRole , (Ver:2.3.8) at: 2021/5/10 16:30:49

        public static List<Entity.ExamRole> Get_All_ExamRole(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRole(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamRole>(reader);
        }

        public static List<Entity.ExamRole> Get_All_ExamRole()
        {
            return Get_All_ExamRole(null);
        }

        public static Entity.ExamRole Get_ExamRole(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRole(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamRole>(reader);
        }

        public static int Insert_ExamRole(Entity.ExamRole info)
        {
            return Insert_ExamRole(info, null, null);
        }

        public static int Insert_ExamRole(Entity.ExamRole info, string[] Include)
        {
            return Insert_ExamRole(info, Include, null);
        }

        public static int Insert_ExamRole(Entity.ExamRole info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamRole(info, Include, Exclude);
        }
        public static int Update_ExamRole(Entity.ExamRole info)
        {
            return Update_ExamRole(info, null, null);
        }

        public static int Update_ExamRole(Entity.ExamRole info, string[] Include)
        {
            return Update_ExamRole(info, Include, null);
        }

        public static int Update_ExamRole(Entity.ExamRole info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamRole(info, Include, Exclude);
        }
        public static int Delete_ExamRole(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamRole(ID);
        }
        #endregion
    }
}