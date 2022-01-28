using System;
using System.Data;
using System.Linq;
using advt.Entity;
using System.Collections.Generic;
using advt.Model.ExamModel;

namespace advt.Data
{
    public partial class ElectronicUser
    {

        #region ElectronicUser , (Ver:2.3.8) at: 2022/1/27 15:33:19

        public static List<Entity.ElectronicUser> Get_All_ElectronicUser(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ElectronicUser(objparams);
            return SqlHelper.GetReaderToList<Entity.ElectronicUser>(reader);
        }

        public static List<Entity.ElectronicUser> Get_All_ElectronicUser()
        {
            return Get_All_ElectronicUser(null);
        }

        public static Entity.ElectronicUser Get_ElectronicUser(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ElectronicUser(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ElectronicUser>(reader);
        }
        public static List<ElectronicUserView> Get_All_ElectronicUserView()
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ElectronicUserView();
            return SqlHelper.GetReaderToList<ElectronicUserView>(reader);
        }

        public static Entity.ElectronicUser Get_ElectronicUser(int ID)
        {
            return Get_ElectronicUser(new { ID = ID });
        }

        public static int Insert_ElectronicUser(Entity.ElectronicUser info)
        {
            return Insert_ElectronicUser(info, null, null);
        }


        public static int Insert_ElectronicUser(Entity.ElectronicUser info, string[] Include)
        {
            return Insert_ElectronicUser(info, Include, null);
        }

        public static int Insert_ElectronicUser(Entity.ElectronicUser info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ElectronicUser(info, Include, Exclude);
        }
        public static int Insert_ElectronicUser_usercode(string usercode, string UserCostCenter, string SubjectName,string username)
        {
            return DatabaseProvider.GetInstance().Insert_ElectronicUser_usercode(usercode, UserCostCenter, SubjectName, username);
        }
        public static int Update_ElectronicUser(Entity.ElectronicUser info)
        {
            return Update_ElectronicUser(info, null, null);
        }

        public static int Update_ElectronicUser(Entity.ElectronicUser info, string[] Include)
        {
            return Update_ElectronicUser(info, Include, null);
        }

        public static int Update_ElectronicUser(Entity.ElectronicUser info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ElectronicUser(info, Include, Exclude);
        }

        public static int Delete_ElectronicUser(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ElectronicUser(ID);
        }
        #endregion
    }
}