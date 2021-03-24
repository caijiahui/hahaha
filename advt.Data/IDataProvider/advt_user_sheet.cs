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
        #region advt_user_sheet , (Ver:2.3.8) at: 2021/3/18 14:35:01

        IDataReader Get_All_advt_user_sheet(object objparams);
        IDataReader Get_advt_user_sheet_UserJobTitle(string username);

        int Insert_advt_user_sheet(Entity.advt_user_sheet info, string[] Include, string[] Exclude);

        #endregion
    }
}
