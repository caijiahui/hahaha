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
        #region ExamScore , (Ver:2.3.8) at: 2021/1/30 14:23:24

        IDataReader Get_All_ExamScore(object objparams);

        int Insert_ExamScore(Entity.ExamScore info, string[] Include, string[] Exclude);

        int Update_ExamScore(Entity.ExamScore info, string[] Include, string[] Exclude);

        int Delete_ExamScore(int ExamID);
        IDataReader Get_All_ExamGetScore(string CreateUser, bool IsTest=false);

        #endregion
    }
}
