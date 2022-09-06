using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data.SqlServer
{
    public partial class DataProvider : advt.Data.IDataProvider
    {
        #region Var: 
        private string[] ExamPostRule_key_a = { "ID" };
        private string ExamPostRule_item_str = "ID,PostID,PostName,DepartCode,RuleName,IsRuleAch,IsRuleQuality,IsRuleShangGang,CreateUser,CreateDate,UpdateUser,UpdateDate,PostExamEntry";
        private string[][] ExamPostRule_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] { "PostID", "NVarChar", "50"},
            new string[] { "PostName", "NVarChar", "50"},
            new string[] { "DepartCode", "NVarChar", "50"},
            new string[] { "RuleName", "NVarChar", "50"},
            new string[] { "IsRuleAch", "Bit", "1"},
            new string[] { "IsRuleQuality", "Bit", "1"},
            new string[] { "IsRuleShangGang", "Bit", "1"},
            new string[] { "CreateUser", "NVarChar", "50"},
            new string[] { "CreateDate", "DateTime", "16"},
            new string[] { "UpdateUser", "NVarChar", "50"},
            new string[] { "UpdateDate", "DateTime", "16"},
            new string[] { "PostExamEntry", "NVarChar", "50"}

        };
        #endregion

        public IDataReader Get_All_ExamPostRule(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamPostRule_item_str + "");
            commandText.AppendLine("   FROM [ExamPostRule]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamPostRule_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamPostRule(Entity.ExamPostRule info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamPostRule_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamPostRule] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamPostRule(Entity.ExamPostRule info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamPostRule_key_a, ExamPostRule_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamPostRule]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamPostRule_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamPostRule(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamPostRule]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamPostRule_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
    }
}
