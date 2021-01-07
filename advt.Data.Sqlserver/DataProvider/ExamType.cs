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
        #region ExamType , (Ver:2.3.8) at: 2021/1/7 15:58:19
        #region Var: 
        private string[] ExamType_key_a = { "ID" };
        private string ExamType_item_str = "[ID],[TypeName],[CreateUser],[CreateDate]";
        private string[][] ExamType_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"TypeName", "NText", "2147483646"},
            new string[] {"CreateUser", "NText", "2147483646"},
            new string[] {"CreateDate", "DateTime", "16"}
        };
        #endregion

        public IDataReader Get_All_ExamType(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamType_item_str + "");
            commandText.AppendLine("   FROM [ExamType]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamType_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamType(Entity.ExamType info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamType_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamType] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamType(Entity.ExamType info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamType_key_a, ExamType_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamType]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamType_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamType(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamType]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamType_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}
