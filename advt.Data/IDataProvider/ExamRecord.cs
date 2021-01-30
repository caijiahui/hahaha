using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region ExamRecord , (Ver:2.3.8) at: 2021/1/30 14:28:04

        IDataReader Get_All_ExamRecord(object objparams);

        int Insert_ExamRecord(Entity.ExamRecord info, string[] Include, string[] Exclude);

        int Update_ExamRecord(Entity.ExamRecord info, string[] Include, string[] Exclude);

        int Delete_ExamRecord(int ID);

        #endregion
    }
}
