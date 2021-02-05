using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region PracticeInfo , (Ver:2.3.8) at: 2021/2/5 11:41:00

        IDataReader Get_All_PracticeInfo(object objparams);

        int Insert_PracticeInfo(Entity.PracticeInfo info, string[] Include, string[] Exclude);

        int Update_PracticeInfo(Entity.PracticeInfo info, string[] Include, string[] Exclude);

        int Delete_PracticeInfo(int ID);

        #endregion
    }
}
