using System;
using System.Data;

using System.Collections.Generic;
using advt.Entity;

namespace advt.Data
{

    public partial interface IDataProvider
    {
        #region ExamBank , (Ver:2.3.8) at: 2021/1/12 15:44:47

        IDataReader Get_All_ExamBank(object objparams);
        IDataReader Get_All_ExamBank_ExamType_Rule(string TopicType, string TopicMajor, string TopicLevel,string ExamSubject);

        int Insert_ExamBank(Entity.ExamBank info, string[] Include, string[] Exclude);

        int Update_ExamBank(Entity.ExamBank info, string[] Include, string[] Exclude);

        int Delete_ExamBank(int ID);

        #endregion
    }
}
