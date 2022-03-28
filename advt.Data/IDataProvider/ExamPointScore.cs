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
        #region ExamPointScore

        IDataReader Get_All_ExamPointScore(object objparams);

        int Insert_ExamPointScore(Entity.ExamPointScore info, string[] Include, string[] Exclude);

        int Update_ExamPointScore(Entity.ExamPointScore info, string[] Include, string[] Exclude);

        int Delete_ExamPointScore(int ID);

       #endregion
    }
}
