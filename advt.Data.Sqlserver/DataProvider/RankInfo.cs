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
        #region RankInfo , (Ver:2.3.8) at: 2021/2/5 11:33:07
        #region Var: 
        private string[] RankInfo_key_a = { "ID" };
        private string RankInfo_item_str = "[ID],[RankName],[SkillName],[MaxSkillName],[CreateUser],[CreateDate],[UpdateUser],[UpdateDate],CorrespondingLevel,NewPromotion,PromotionBonus";
        private string[][] RankInfo_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"RankName", "NVarChar", "500"},
            new string[] {"SkillName", "NVarChar", "500"},
            new string[] {"MaxSkillName", "NVarChar", "500"},
            new string[] {"CreateUser", "NVarChar", "500"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] {"UpdateUser", "NVarChar", "500"},
            new string[] {"UpdateDate", "DateTime", "16"},
            new string[] {"CorrespondingLevel", "NVarChar", "500"},
            new string[] {"NewPromotion", "NVarChar", "500"},
            new string[] {"PromotionBonus", "Int", "500"}
        };
        #endregion

        public IDataReader Get_All_RankInfo(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + RankInfo_item_str + "");
            commandText.AppendLine("   FROM [RankInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(RankInfo_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_RankInfo(Entity.RankInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(RankInfo_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [RankInfo] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_RankInfo(Entity.RankInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(RankInfo_key_a, RankInfo_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [RankInfo]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(RankInfo_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_RankInfo(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [RankInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(RankInfo_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}
