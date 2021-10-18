using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial class RegionalPost
    {

        #region RegionalPost , (Ver:2.3.8) at: 2021/1/7 16:05:32

        public static List<Entity.RegionalPost> Get_All_RegionalPost(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_RegionalPost(objparams);
            return SqlHelper.GetReaderToList<Entity.RegionalPost>(reader);
        }

        public static List<Entity.RegionalPost> Get_All_RegionalPost()
        {
            return Get_All_RegionalPost(null);
        }

        public static Entity.RegionalPost Get_RegionalPost(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_RegionalPost(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.RegionalPost>(reader);
        }

        public static Entity.RegionalPost Get_RegionalPost(int ID)
        {
            return Get_RegionalPost(new { ID = ID });
        }

        public static int Insert_RegionalPost(Entity.RegionalPost info)
        {
            return Insert_RegionalPost(info, null, null);
        }

        public static int Insert_RegionalPost(Entity.RegionalPost info, string[] Include)
        {
            return Insert_RegionalPost(info, Include, null);
        }

        public static int Insert_RegionalPost(Entity.RegionalPost info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_RegionalPost(info, Include, Exclude);
        }

        public static int Update_RegionalPost(Entity.RegionalPost info)
        {
            return Update_RegionalPost(info, null, null);
        }

        public static int Update_RegionalPost(Entity.RegionalPost info, string[] Include)
        {
            return Update_RegionalPost(info, Include, null);
        }

        public static int Update_RegionalPost(Entity.RegionalPost info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_RegionalPost(info, Include, Exclude);
        }

        public static int Delete_RegionalPost(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_RegionalPost(ID);
        }

        public static List<Entity.RegionalPost> Get_All_RegionalPostInfo(string RuleName,string PostName)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_RegionalPostInfo(RuleName, PostName);
            return SqlHelper.GetReaderToList<Entity.RegionalPost>(reader);
        }
        
    }
    #endregion
}
