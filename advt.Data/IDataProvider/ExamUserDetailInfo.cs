﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial interface IDataProvider
    {
        #region ExamUserDetailInfo , (Ver:2.3.8) at: 2021/2/5 11:28:28

        IDataReader Get_All_ExamUserDetailInfo(object objparams);

        int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude);

        int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude);

        int Delete_ExamUserDetailInfo(int ID);

        #endregion
    }
}