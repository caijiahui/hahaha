using System;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using advt.Common;
using System.Collections.Generic;
using advt.Data;
using advt.Entity;

namespace advt.Data.SqlServer
{
    public partial class DataProvider : advt.Data.IDataProvider
    {
        #region ExamUserDetailInfo , (Ver:2.3.8) at: 2021/3/4 9:31:04
        #region Var: 
        private string[] ExamUserDetailInfo_key_a = { "ID" };
        private string ExamUserDetailInfo_item_str = "[ID],[UserCode],[UserName],[DepartCode],[PostName],[RankName],[SkillName],[EntryDate],[Achievement],[ExamDate],[ExamScore],[PracticeScore],[PlanExamDate],[ExamPlace],[ExamStatus],[IsReview],[RuleName],[SubjectName],[TypeName],[ApplyLevel],[HighestLevel],[IsAchievement],[IsStop],[IsExam],[HrCreateUser],[HrCreateDate],[DirectorCreateUser],[DirectorCreateDate],[HrCheckCreateUser],[HrCheckCreateDate],[StopCreateUser],[StopCreateDate],UserExamDate";
        private string[][] ExamUserDetailInfo_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"UserCode", "NVarChar", "500"},
            new string[] {"UserName", "NVarChar", "500"},
            new string[] {"DepartCode", "NVarChar", "500"},
            new string[] {"PostName", "NVarChar", "500"},
            new string[] {"RankName", "NVarChar", "500"},
            new string[] {"SkillName", "NVarChar", "500"},
            new string[] {"EntryDate", "DateTime", "16"},
            new string[] {"Achievement", "NVarChar", "500"},
            new string[] {"ExamDate", "DateTime", "16"},
            new string[] {"ExamScore", "Decimal", "20"},
            new string[] {"PracticeScore", "Decimal", "20"},
            new string[] {"PlanExamDate", "DateTime", "16"},
            new string[] {"ExamPlace", "NVarChar", "500"},
            new string[] {"ExamStatus", "NVarChar", "500"},
            new string[] {"IsReview", "Bit", "1"},
            new string[] {"RuleName", "NVarChar", "500"},
            new string[] {"SubjectName", "NVarChar", "500"},
            new string[] {"TypeName", "NVarChar", "500"},
            new string[] {"ApplyLevel", "NVarChar", "500"},
            new string[] {"HighestLevel", "NVarChar", "500"},
            new string[] {"IsAchievement", "Bit", "1"},
            new string[] {"IsStop", "Bit", "1"},
            new string[] {"IsExam", "NVarChar", "500"},
             new string[] { "HrCreateUser", "NVarChar", "500"},
            new string[] { "HrCreateDate", "DateTime", "16"},
              new string[] { "DirectorCreateUser", "NVarChar", "500"},
            new string[] { "DirectorCreateDate", "DateTime", "16"},
              new string[] { "HrCheckCreateUser", "NVarChar", "500"},
            new string[] { "HrCheckCreateDate", "DateTime", "16"},
              new string[] { "StopCreateUser", "NVarChar", "500"},
            new string[] { "StopCreateDate", "DateTime", "16"},
             new string[] { "UserExamDate", "DateTime", "16"}
        };
        #endregion

        public IDataReader Get_All_ExamUserDetailInfo(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamUserDetailInfo_item_str + "");
            commandText.AppendLine("   FROM [ExamUserDetailInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamUserDetailInfo_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamUserDetailInfo_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamUserDetailInfo] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamUserDetailInfo_key_a, ExamUserDetailInfo_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamUserDetailInfo]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUserDetailInfo_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamUserDetailInfo(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamUserDetailInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUserDetailInfo_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}
