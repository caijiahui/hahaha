using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial class AreaPost
    {
        public static List<Entity.AreaPost> Get_All_AreaPost(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_AreaPost(objparams);
            return SqlHelper.GetReaderToList<Entity.AreaPost>(reader);
        }

        public static List<Entity.AreaPost> Get_All_AreaPost()
        {
            return Get_All_AreaPost(null);
        }

        public static List<Entity.AreaPost> Get_All_AreaPostArea()
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_AreaPostArea();
            return SqlHelper.GetReaderToList<Entity.AreaPost>(reader);
        }
        public static List<Entity.AreaPost> Get_All_AreaPostDept(string model)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_AreaPostDept(model);
            return SqlHelper.GetReaderToList<Entity.AreaPost>(reader);
        }
        public static List<Entity.AreaPost> Get_All_AreaPostName(string model)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_AreaPostName(model);
            return SqlHelper.GetReaderToList<Entity.AreaPost>(reader);
        }
        


    }
}
