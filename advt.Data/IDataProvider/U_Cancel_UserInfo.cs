using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region U_Cancel_UserInfo , (Ver:2.3.8) at: 2021/2/5 11:37:14

        IDataReader Get_All_U_Cancel_UserInfo(object objparams);

        int Insert_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info, string[] Include, string[] Exclude);

        int Update_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info, string[] Include, string[] Exclude);

        int Delete_U_Cancel_UserInfo(int ID);
        IDataReader GetAllCancelUserInfo(string SubjectName, string searchdata);

        #endregion
    }
}
