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
        #region ExamUserInfo , (Ver:2.3.8) at: 2021/2/5 11:25:22
        #region Var: 
        private string[] ExamUserInfo_key_a = { "ID" };
        private string ExamUserInfo_item_str = "[ID],[UserCode],[UserName],[DepartCode],[PostName],[RankName],[EntryDate],[Achievement],[TypeName],[SubjectName],[CreateUser],[CreateDate],[UpdateUser],[UpdateDate]";
        private string[][] ExamUserInfo_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"UserCode", "NVarChar", "500"},
            new string[] {"UserName", "NVarChar", "500"},
            new string[] {"DepartCode", "NVarChar", "500"},
            new string[] {"PostName", "NVarChar", "500"},
            new string[] {"RankName", "NVarChar", "500"},
            new string[] {"EntryDate", "DateTime", "16"},
            new string[] {"Achievement", "NVarChar", "500"},
            new string[] {"TypeName", "NVarChar", "500"},
            new string[] {"SubjectName", "NVarChar", "500"},
            new string[] {"CreateUser", "NVarChar", "500"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] {"UpdateUser", "NVarChar", "500"},
            new string[] {"UpdateDate", "DateTime", "16"}
        };
        #endregion

        public IDataReader Get_All_ExamUserInfo(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamUserInfo_item_str + "");
            commandText.AppendLine("   FROM [ExamUserInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamUserInfo_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamUserInfo(Entity.ExamUserInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamUserInfo_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamUserInfo] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamUserInfo(Entity.ExamUserInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamUserInfo_key_a, ExamUserInfo_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamUserInfo]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUserInfo_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamUserInfo(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamUserInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUserInfo_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Get_UpdateExamUserInfo(string UserCode, string Achievement)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            commandText.AppendLine(" update ExamUserInfo set Achievement=@Achievement where UserCode=@UserCode");
            l_parms.Add(SqlHelper.MakeInParam("@Achievement", (DbType)SqlDbType.NVarChar, 150, Achievement));
            l_parms.Add(SqlHelper.MakeInParam("@UserCode", (DbType)SqlDbType.NVarChar, 150, UserCode));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());

        }


        #endregion
    }
}
