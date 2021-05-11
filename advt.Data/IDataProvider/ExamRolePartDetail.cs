using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {

        #region ExamRolePartDetail , (Ver:2.3.8) at: 2021/5/11 8:40:48

        IDataReader Get_All_ExamRolePartDetail(object objparams);

        int Insert_ExamRolePartDetail(Entity.ExamRolePartDetail info, string[] Include, string[] Exclude);

        int Update_ExamRolePartDetail(Entity.ExamRolePartDetail info, string[] Include, string[] Exclude);

        int Delete_ExamRolePartDetail(int ID);

        #endregion
    }
}
