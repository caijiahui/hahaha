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

        #region ElectronicUser , (Ver:2.3.8) at: 2022/1/27 15:33:19

        IDataReader Get_All_ElectronicUser(object objparams);
        IDataReader Get_All_ElectronicUserView();
        
        int Insert_ElectronicUser_usercode(string usercode, string UserCostCenter, string SubjectName,string username);
        int Insert_ElectronicUser(Entity.ElectronicUser info, string[] Include, string[] Exclude);

        int Update_ElectronicUser(Entity.ElectronicUser info, string[] Include, string[] Exclude);

        int Delete_ElectronicUser(int ID);

        #endregion
    }
}
