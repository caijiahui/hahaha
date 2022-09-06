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
        IDataReader Get_All_ExamPostRule(object objparams);

        int Insert_ExamPostRule(Entity.ExamPostRule info, string[] Include, string[] Exclude);

        int Update_ExamPostRule(Entity.ExamPostRule info, string[] Include, string[] Exclude);

        int Delete_ExamPostRule(int ID);
    }
}
