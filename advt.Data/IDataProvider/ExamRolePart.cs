using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {

        #region ExamRolePart , (Ver:2.3.8) at: 2021/5/10 16:25:41

        IDataReader Get_All_ExamRolePart(object objparams);

        int Insert_ExamRolePart(Entity.ExamRolePart info, string[] Include, string[] Exclude);
        int Delete_ExamRolePart(int ID); 
              int Delete_ExamRolePartByName(string rolename);
        #endregion
    }
}