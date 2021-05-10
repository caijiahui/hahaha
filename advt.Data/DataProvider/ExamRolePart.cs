using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamRolePart
    {

        #region ExamRolePart , (Ver:2.3.8) at: 2021/5/10 16:25:41

        public static List<Entity.ExamRolePart> Get_All_ExamRolePart(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRolePart(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamRolePart>(reader);
        }

        public static List<Entity.ExamRolePart> Get_All_ExamRolePart()
        {
            return Get_All_ExamRolePart(null);
        }

        public static Entity.ExamRolePart Get_ExamRolePart(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamRolePart(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamRolePart>(reader);
        }

        public static int Insert_ExamRolePart(Entity.ExamRolePart info)
        {
            return Insert_ExamRolePart(info, null, null);
        }

        public static int Insert_ExamRolePart(Entity.ExamRolePart info, string[] Include)
        {
            return Insert_ExamRolePart(info, Include, null);
        }

        public static int Insert_ExamRolePart(Entity.ExamRolePart info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamRolePart(info, Include, Exclude);
        }
        public static int Delete_ExamRolePart(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamRolePart(ID);
        }
        public static int Delete_ExamRolePartByName(string rolename)
        {
            return DatabaseProvider.GetInstance().Delete_ExamRolePartByName(rolename);
        }
        #endregion
    }
}