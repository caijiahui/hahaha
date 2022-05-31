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
        #region AchieveRecord , (Ver:2.3.8) at: 2021/2/5 11:41:00
        #region Var: 
        private string[] AchieveRecord_key_a = { "ID" };
        private string AchieveRecord_item_str = "ID,UserCode,CreateUser,CreateDate,Achievement,RecordType";
        private string[][] AchieveRecord_item_prop_a =
        {
            new string[] { "ID", "Int", "4"},
            new string[] { "UserCode", "NVarChar", "50"},
            new string[] { "CreateUser", "NVarChar", "50"},
            new string[] { "CreateDate", "DateTime", "16"},
            new string[] { "Achievement", "NVarChar", "50"},
            new string[] { "RecordType", "NVarChar", "50"}
             
        };
        #endregion

        public IDataReader Get_All_AchieveRecord(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + AchieveRecord_item_str + "");
            commandText.AppendLine("   FROM [AchieveRecord]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(AchieveRecord_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
       
        public int Insert_AchieveRecord(Entity.AchieveRecord info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(AchieveRecord_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [AchieveRecord] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_AchieveRecord(Entity.AchieveRecord info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(AchieveRecord_key_a, AchieveRecord_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [AchieveRecord]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(AchieveRecord_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_AchieveRecord(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [AchieveRecord]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(AchieveRecord_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }


        public IDataReader Get_All_Record(string startdate, string endate)
        {
            StringBuilder commandText = new StringBuilder();           
            commandText.AppendLine("select * from ExamUserInfo  b inner join (select * from AchieveRecord where CreateDate >= '"+startdate+ "' and CreateDate < '" + endate + "') a on a.UserCode = b.UserCode and a.Achievement = N'符合'and b.TypeName = N'Chassis技能等级考试'");
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        #endregion
    }
}
