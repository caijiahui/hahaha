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
        #region ExamPointScore , (Ver:2.3.8) at: 2021/1/7 16:05:32
        #region Var: 
        private string[] ExamPointScore_key_a = { "ID" };
        private string ExamPointScore_item_str = "[ID],UserCode,UserName,PointScore,Year,Month,CreateUser,CreateDate,UpdateUser,UpdateDate";
        private string[][] ExamPointScore_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] { "UserCode", "NVarChar", "50"},
            new string[] { "UserName", "NVarChar", "50"},
            new string[] { "PointScore", "NVarChar", "50"},
            new string[] { "Year", "NVarChar", "50"},
            new string[] { "Month", "NVarChar", "50"},
            new string[] { "CreateUser", "NVarChar", "50"},
            new string[] { "CreateDate", "DateTime", "16"},
            new string[] { "UpdateUser", "NVarChar", "50"},
            new string[] { "UpdateDate", "DateTime", "16"}

        };
        #endregion

        public IDataReader Get_All_ExamPointScore(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamPointScore_item_str + "");
            commandText.AppendLine("   FROM [ExamPointScore]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamPointScore_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamPointScore(Entity.ExamPointScore info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamPointScore_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamPointScore] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamPointScore(Entity.ExamPointScore info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamPointScore_key_a, ExamPointScore_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamPointScore]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamPointScore_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamPointScore(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamPointScore]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamPointScore_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        #endregion
    }
}

