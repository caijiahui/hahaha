using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial interface IDataProvider
    {
        #region AchieveRecord , (Ver:2.3.8) at: 2021/2/5 11:41:00

        IDataReader Get_All_AchieveRecord(object objparams);

        int Insert_AchieveRecord(Entity.AchieveRecord info, string[] Include, string[] Exclude);

        int Update_AchieveRecord(Entity.AchieveRecord info, string[] Include, string[] Exclude);

        int Delete_AchieveRecord(int ID);

        #endregion
    }
}
