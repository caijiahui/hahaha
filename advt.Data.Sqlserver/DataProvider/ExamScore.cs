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
        #region ExamScore , (Ver:2.3.8) at: 2021/1/30 14:23:24
        #region Var: 
        private string[] ExamScore_key_a = { "ExamID" };
        private string ExamScore_item_str = "[ExamID],[ExamType],[IsTest],[CreateUser],[CreateDate],[CorrectNum],[CorrectScore],[TotalScore]";
        private string[][] ExamScore_item_prop_a =
        {
            new string[] {"ExamID", "Int", "4"},
            new string[] {"ExamType", "NVarChar", "500"},
            new string[] {"IsTest", "Bit", "1"},
            new string[] {"CreateUser", "NVarChar", "500"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] {"CorrectNum", "Int", "4"},
             new string[] { "CorrectScore", "Decimal", "20"},
              new string[] { "TotalScore", "Decimal", "20"}
        };
        #endregion

        public IDataReader Get_All_ExamScore(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamScore_item_str + "");
            commandText.AppendLine("   FROM [ExamScore]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamScore_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamScore(Entity.ExamScore info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamScore_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamScore] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamScore(Entity.ExamScore info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamScore_key_a, ExamScore_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamScore]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamScore_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamScore(int ExamID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamScore]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamScore_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ExamID", (DbType)SqlDbType.Int, 4, ExamID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}
