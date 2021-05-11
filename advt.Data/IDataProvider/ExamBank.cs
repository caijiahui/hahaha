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
        IDataReader Get_All_ExamBank_ExamType_Subject(string ExamType, string ExamSubject, string ExamMajor, string ExamLevel, string ExamContent);

        int Insert_ExamBank(Entity.ExamBank info, string[] Include, string[] Exclude);

        int Update_ExamBank(Entity.ExamBank info, string[] Include, string[] Exclude);

        int Delete_ExamBank(int ID);
        int Delete_ExamBank_TypeName_ExamSubject_Level(string TypeName, string ExamSubject, string TopicLevel);

        #endregion
    }
}
