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
        #region RegionalPost , (Ver:2.3.8) at: 2021/1/7 16:05:32
        #region Var: 
        private string[] RegionalPost_key_a = { "ID" };
        private string RegionalPost_item_str = "[ID],[RegionalPlace],[DepartCode],[PostID],[PostName],RuleName,RuleTwoName,ExamType";
        private string[][] RegionalPost_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] { "RegionalPlace", "NVarChar", "500"},
            new string[] { "DepartCode", "NVarChar", "500"},
            new string[] { "PostID", "NVarChar", "500"},
            new string[] { "PostName", "NVarChar", "500"},
            new string[] { "RuleName", "NVarChar", "500"},
            new string[] { "RuleTwoName", "NVarChar", "500"},
            new string[] { "ExamType", "NVarChar", "500"}

        };
        #endregion

        public IDataReader Get_All_RegionalPost(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + RegionalPost_item_str + "");
            commandText.AppendLine("   FROM [RegionalPost]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(RegionalPost_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_RegionalPost(Entity.RegionalPost info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(RegionalPost_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [RegionalPost] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_RegionalPost(Entity.RegionalPost info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(RegionalPost_key_a, RegionalPost_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [RegionalPost]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamSubject_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_RegionalPost(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [RegionalPost]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(RegionalPost_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_RegionalPostInfo(string PostName, string RuleName,string RuleTwoName)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" Update [RegionalPost] set RuleName=@RuleName,RuleTwoName=@RuleTwoName where PostName=@PostName");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@RuleName", (DbType)SqlDbType.NVarChar, 150, RuleName));
            l_parms.Add(SqlHelper.MakeInParam("@PostName", (DbType)SqlDbType.NVarChar, 150, PostName));
            l_parms.Add(SqlHelper.MakeInParam("@RuleTwoName", (DbType)SqlDbType.NVarChar, 150, RuleTwoName));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }


        public IDataReader Get_All_RegionalPostInfo(string RuleName, string PostName,string RuleTwoName)
        {
            StringBuilder commandText = new StringBuilder();
            var texts = "";
            commandText.AppendLine(" SELECT " + RegionalPost_item_str + "");
            commandText.AppendLine("   FROM [RegionalPost] where 1=1 ");
            if (!string.IsNullOrEmpty(RuleName))
            {
                texts += " and RuleName like  N'%" + RuleName + "%'";
            }
            if (!string.IsNullOrEmpty(PostName))
            {
                texts += " and PostName like  N'%" + PostName + "%'";
            }
            if (!string.IsNullOrEmpty(RuleTwoName))
            {
                texts += " and RuleTwoName like  N'%" + RuleTwoName + "%'";
            }
            commandText.AppendLine(texts);
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@RuleName", (DbType)SqlDbType.NVarChar, 150, RuleName));
            l_parms.Add(SqlHelper.MakeInParam("@PostName", (DbType)SqlDbType.NVarChar, 150, PostName));
            l_parms.Add(SqlHelper.MakeInParam("@RuleTwoName", (DbType)SqlDbType.NVarChar, 150, RuleTwoName));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        #endregion
    }
}
