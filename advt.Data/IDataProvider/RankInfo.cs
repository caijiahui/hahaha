using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region RankInfo , (Ver:2.3.8) at: 2021/2/5 11:33:07

        IDataReader Get_All_RankInfo(object objparams);

        int Insert_RankInfo(Entity.RankInfo info, string[] Include, string[] Exclude);

        int Update_RankInfo(Entity.RankInfo info, string[] Include, string[] Exclude);

        int Delete_RankInfo(int ID);

        #endregion
    }
}
