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

        #region ExamRolePart , (Ver:2.3.8) at: 2021/5/10 16:25:41
        #region Var: 
        private string[] ExamRolePart_key_a = { "ID" };
        private string ExamRolePart_item_str = "[ID],[RoleName],[PartName]";
        private string[][] ExamRolePart_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] { "RoleName", "NVarChar", "500"},
            new string[] {"PartName", "NVarChar", "500"}
        };
        #endregion

        public IDataReader Get_All_ExamRolePart(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamRolePart_item_str + "");
            commandText.AppendLine("   FROM [ExamRolePart]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamRolePart_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_ExamRolPartDetail_Sort(string username, string Action, string Controller)
        {
            StringBuilder commandText = new StringBuilder();
            var text = "";
            if (!string.IsNullOrEmpty(username))
            {
                text += " and d.username = '" + username + "'";
            }
            if (!string.IsNullOrEmpty(Action))
            {
                text += " and c.[Action] ='" + Action + "'";
            }
            if (!string.IsNullOrEmpty(Controller))
            {
                text += " and c.[Controller] ='" + Controller + "'";
            }
            commandText.AppendLine("select b.PartName,c.[Action],c.Controller,c.Sort from ExamRole a inner join ExamRolePart b on a.RoleName=b.RoleName  " +
 " inner join ExamRolePartDetail c on b.PartName = c.PartName " +
 " inner join advt_users_type d on d.type = a.RoleName" +
" where 1=1 " + text + " order by Sort asc");
           
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        
        public int Insert_ExamRolePart(Entity.ExamRolePart info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamRolePart_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamRolePart] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public int Delete_ExamRolePart(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamRolePart]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRolePart_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public int Delete_ExamRolePartByName(string rolename)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamRolePart]");
            commandText.AppendLine(" where RoleName=@rolename   ");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@rolename", (DbType)SqlDbType.NVarChar, 500, rolename));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        
        #endregion
    }
}