using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region RegionalPost , (Ver:2.3.8) at: 2021/1/7 16:05:32

        IDataReader Get_All_RegionalPost(object objparams);

        int Insert_RegionalPost(Entity.RegionalPost info, string[] Include, string[] Exclude);

        int Update_RegionalPost(Entity.RegionalPost info, string[] Include, string[] Exclude);

        int Delete_RegionalPost(int ID);
    
        #endregion
    }
}
