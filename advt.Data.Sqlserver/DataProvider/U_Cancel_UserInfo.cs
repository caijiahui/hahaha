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
        #region U_Cancel_UserInfo , (Ver:2.3.8) at: 2021/2/5 11:37:14
        #region Var: 
        private string[] U_Cancel_UserInfo_key_a = { "ID" };
        private string U_Cancel_UserInfo_item_str = "[ID],[HC],[UserCode],[UserName],[DepartCode],[ExamDate],[SubjectName],PostID,STATE,InsertDate";
        private string[][] U_Cancel_UserInfo_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] { "HC", "Int", "4"},
            new string[] { "UserCode", "NVarChar", "50"},
            new string[] { "UserName", "NVarChar", "50"},
            new string[] { "DepartCode", "NVarChar", "50"},
            new string[] { "ExamDate", "DateTime", "16"},
             new string[] { "SubjectName", "NVarChar", "50"},
             new string[] { "PostID", "NVarChar", "50"},
              new string[] { "STATE", "NVarChar", "50"}, new string[] { "InsertDate", "DateTime", "16"},
        };
        #endregion

        public IDataReader Get_All_U_Cancel_UserInfo(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + U_Cancel_UserInfo_item_str + "");
            commandText.AppendLine("   FROM [U_Cancel_UserInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(U_Cancel_UserInfo_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(U_Cancel_UserInfo_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [U_Cancel_UserInfo] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_U_Cancel_UserInfo(Entity.U_Cancel_UserInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(U_Cancel_UserInfo_key_a, U_Cancel_UserInfo_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [U_Cancel_UserInfo]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(U_Cancel_UserInfo_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_U_Cancel_UserInfo(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [U_Cancel_UserInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(U_Cancel_UserInfo_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader GetAllCancelUserInfo(string SubjectName, string searchdata)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from U_Cancel_UserInfo where  SubjectName=N'" + SubjectName + "'");           
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }

        #endregion

    }
}
