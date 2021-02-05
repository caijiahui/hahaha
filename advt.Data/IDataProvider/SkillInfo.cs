using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region SkillInfo , (Ver:2.3.8) at: 2021/2/5 11:37:14

        IDataReader Get_All_SkillInfo(object objparams);

        int Insert_SkillInfo(Entity.SkillInfo info, string[] Include, string[] Exclude);

        int Update_SkillInfo(Entity.SkillInfo info, string[] Include, string[] Exclude);

        int Delete_SkillInfo(int ID);

        #endregion
    }
}
