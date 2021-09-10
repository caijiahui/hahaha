using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region ExamUserInfo , (Ver:2.3.8) at: 2021/2/5 11:25:22

        IDataReader Get_All_ExamUserInfo(object objparams);

        int Insert_ExamUserInfo(Entity.ExamUserInfo info, string[] Include, string[] Exclude);

        int Update_ExamUserInfo(Entity.ExamUserInfo info, string[] Include, string[] Exclude);

        int Delete_ExamUserInfo(int ID);
        int Get_UpdateExamUserInfo(string UserCode, string Achievement);
        int ReverseExamUserInfo(int ID,string Level,string username);
        #endregion
    }
}
