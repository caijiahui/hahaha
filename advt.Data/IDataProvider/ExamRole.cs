using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {

        #region ExamRole , (Ver:2.3.8) at: 2021/5/10 16:19:10

        IDataReader Get_All_ExamRole(object objparams);

        int Insert_ExamRole(Entity.ExamRole info, string[] Include, string[] Exclude);
        int Update_ExamRole(Entity.ExamRole info, string[] Include, string[] Exclude);
        int Delete_ExamRole(int ID);

        #endregion
    }
}
