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
        IDataReader Get_All_UserQuataRecord(object objparams);

        int Insert_UserQuataRecord(Entity.UserQuataRecord info, string[] Include, string[] Exclude);
    }
}
