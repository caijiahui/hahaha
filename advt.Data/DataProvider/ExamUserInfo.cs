using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;


namespace advt.Data
{
    public partial class ExamUserInfo
    {

        #region ExamUserInfo , (Ver:2.3.8) at: 2021/2/5 11:25:22

        public static List<Entity.ExamUserInfo> Get_All_ExamUserInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserInfo(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamUserInfo>(reader);
        }

        public static List<Entity.ExamUserInfo> Get_All_ExamUserInfo()
        {
            return Get_All_ExamUserInfo(null);
        }

        public static Entity.ExamUserInfo Get_ExamUserInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserInfo(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamUserInfo>(reader);
        }

        public static Entity.ExamUserInfo Get_ExamUserInfo(int ID)
        {
            return Get_ExamUserInfo(new { ID = ID });
        }

        public static int Insert_ExamUserInfo(Entity.ExamUserInfo info)
        {
            return Insert_ExamUserInfo(info, null, null);
        }

        public static int Insert_ExamUserInfo(Entity.ExamUserInfo info, string[] Include)
        {
            return Insert_ExamUserInfo(info, Include, null);
        }

        public static int Insert_ExamUserInfo(Entity.ExamUserInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamUserInfo(info, Include, Exclude);
        }

        public static int Update_ExamUserInfo(Entity.ExamUserInfo info)
        {
            return Update_ExamUserInfo(info, null, null);
        }

        public static int Update_ExamUserInfo(Entity.ExamUserInfo info, string[] Include)
        {
            return Update_ExamUserInfo(info, Include, null);
        }

        public static int Update_ExamUserInfo(Entity.ExamUserInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamUserInfo(info, Include, Exclude);
        }

        public static int Delete_ExamUserInfo(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamUserInfo(ID);
        }

        public static int Get_UpdateExamUserInfo(string UserCode, string Achievement)
        {
            return DatabaseProvider.GetInstance().Get_UpdateExamUserInfo(UserCode, Achievement);
        }
        public static int ReverseExamUserInfo(int ID,string Level,string username)
        {
            return DatabaseProvider.GetInstance().ReverseExamUserInfo(ID, Level, username);
        }
        #endregion
    }
}