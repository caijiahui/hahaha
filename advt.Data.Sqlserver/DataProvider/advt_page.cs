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
        public IDataReader Get_All_advt_page_By_UserGroup(int user_group_id)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT F.*");
            commandText.AppendLine("   FROM [advt_page] F");
            commandText.AppendLine(" INNER JOIN [advt_user_group_mapping] FF ON F.[id]=FF.[pid]");
            commandText.AppendLine(" WHERE FF.[groupid] = @groupid");
            commandText.AppendLine(" AND F.[available] =" + (byte)Entity.Status.Normal.Enable);
            commandText.AppendLine(" AND FF.[available] =" + (byte)Entity.Status.Normal.Enable);
            commandText.AppendLine(" ORDER BY FF.[sort] DESC");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@groupid", (DbType)SqlDbType.Int, 4, user_group_id));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        #region advt_page , (Ver:2.3.8) at: 2018/5/5 17:38:29
        #region Var: 
        private string[] advt_page_key_a = { "id" };
        private string advt_page_item_str = "[id],[name],[parea],[paction],[pcontroller],[description],[available]";
        private string[][] advt_page_item_prop_a = 
        {
            new string[] {"id", "Int", "4"},
            new string[] {"name", "NVarChar", "200"},
            new string[] {"parea", "NVarChar", "50"},
            new string[] {"paction", "NVarChar", "50"},
            new string[] {"pcontroller", "NVarChar", "50"},
            new string[] {"description", "NVarChar", "500"},
            new string[] {"available", "TinyInt", "1"}
        };
        #endregion

        public IDataReader Get_All_advt_page(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + advt_page_item_str + "");
            commandText.AppendLine("   FROM [advt_page]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(advt_page_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_advt_page(Entity.advt_page info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(advt_page_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [advt_page] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_advt_page(Entity.advt_page info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(advt_page_key_a, advt_page_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [advt_page]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(advt_page_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_advt_page(int id)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [advt_page]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(advt_page_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}
