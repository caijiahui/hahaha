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
        #region SkillInfo , (Ver:2.3.8) at: 2021/2/5 11:37:14
        #region Var: 
        private string[] SkillInfo_key_a = { "ID" };
        private string SkillInfo_item_str = "[ID],[SKillName],[AchRequire],[CreateUser],[CreateDate],[UpdateUser],[UpdateDate],SkillAllowance";
        private string[][] SkillInfo_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"SKillName", "NVarChar", "500"},
            new string[] {"AchRequire", "NVarChar", "500"},
            new string[] {"CreateUser", "NVarChar", "500"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] {"UpdateUser", "NVarChar", "500"},
            new string[] {"UpdateDate", "DateTime", "16"},
             new string[] { "SkillAllowance", "Int", "4"}
        };
        #endregion

        public IDataReader Get_All_SkillInfo(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + SkillInfo_item_str + "");
            commandText.AppendLine("   FROM [SkillInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(SkillInfo_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_SkillInfo(Entity.SkillInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(SkillInfo_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [SkillInfo] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_SkillInfo(Entity.SkillInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(SkillInfo_key_a, SkillInfo_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [SkillInfo]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(SkillInfo_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_SkillInfo(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [SkillInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(SkillInfo_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion

    }
}
