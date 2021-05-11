using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {

        #region ExamUsersFromehr , (Ver:2.3.8) at: 2021/5/11 11:39:36

        IDataReader Get_All_ExamUsersFromehr(object objparams);

        int Insert_ExamUsersFromehr(Entity.ExamUsersFromehr info, string[] Include, string[] Exclude);

        int Update_ExamUsersFromehr(Entity.ExamUsersFromehr info, string[] Include, string[] Exclude);

        int Delete_ExamUsersFromehr(string UserCode);

        #endregion
    }
}