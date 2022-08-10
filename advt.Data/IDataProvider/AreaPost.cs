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
        IDataReader Get_All_AreaPost(object objparams);
        IDataReader Get_All_AreaPostArea();
        IDataReader Get_All_AreaPostDept(string model);
        IDataReader Get_All_AreaPostName(string model);
    }
}
