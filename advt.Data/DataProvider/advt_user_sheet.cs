using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial class advt_user_sheet
    {
        #region advt_user_sheet , (Ver:2.3.8) at: 2021/3/18 14:35:01

        public static List<Entity.advt_user_sheet> Get_All_advt_user_sheet(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_advt_user_sheet(objparams);
            return SqlHelper.GetReaderToList<Entity.advt_user_sheet>(reader);
        }
        //根据科目工号部门人名找到电子端可报名人员
        public static List<Entity.advt_user_sheet> Get_All_advt_user_sheet_ElectronicUser(string sdata,string subject)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_advt_user_sheet_ElectronicUser(sdata, subject);
            return SqlHelper.GetReaderToList<Entity.advt_user_sheet>(reader);
        }

        public static List<Entity.advt_user_sheet> Get_All_advt_user_sheet()
        {
            return Get_All_advt_user_sheet(null);
        }

        public static Entity.advt_user_sheet Get_advt_user_sheet(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_advt_user_sheet(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.advt_user_sheet>(reader);
        }
        public static List<Entity.advt_user_sheet> Get_advt_user_sheet_UserJobTitle(string UserAccount)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_advt_user_sheet_UserJobTitle(UserAccount);
            return SqlHelper.GetReaderToList<Entity.advt_user_sheet>(reader);
        }

        public static int Insert_advt_user_sheet(Entity.advt_user_sheet info)
        {
            return Insert_advt_user_sheet(info, null, null);
        }

        public static int Insert_advt_user_sheet(Entity.advt_user_sheet info, string[] Include)
        {
            return Insert_advt_user_sheet(info, Include, null);
        }

        public static int Insert_advt_user_sheet(Entity.advt_user_sheet info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_advt_user_sheet(info, Include, Exclude);
        }

        #endregion
    }
}
