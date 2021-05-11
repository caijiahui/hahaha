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

        #region ExamRolePartDetail , (Ver:2.3.8) at: 2021/5/11 8:40:48
        #region Var: 
        private string[] ExamRolePartDetail_key_a = { "ID" };
        private string ExamRolePartDetail_item_str = "[ID],[PartName],[Controller],[Action],[Sort]";
        private string[][] ExamRolePartDetail_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"PartName", "NVarChar", "500"},
            new string[] {"Controller", "NVarChar", "500"},
            new string[] {"Action", "NVarChar", "500"},
            new string[] {"Sort", "Int", "4"}
        };
        #endregion

        public IDataReader Get_All_ExamRolePartDetail(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamRolePartDetail_item_str + "");
            commandText.AppendLine("   FROM [ExamRolePartDetail]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamRolePartDetail_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamRolePartDetail(Entity.ExamRolePartDetail info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamRolePartDetail_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamRolePartDetail] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamRolePartDetail(Entity.ExamRolePartDetail info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamRolePartDetail_key_a, ExamRolePartDetail_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamRolePartDetail]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRolePartDetail_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamRolePartDetail(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamRolePartDetail]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRolePartDetail_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}