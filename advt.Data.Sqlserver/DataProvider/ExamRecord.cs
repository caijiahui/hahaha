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
        #region ExamRecord , (Ver:2.3.8) at: 2021/1/30 14:28:04
        #region Var: 
        private string[] ExamRecord_key_a = { "ID" };
        private string ExamRecord_item_str = "[ID],[ExamID],[TopicTitle],[TopicTitlePicNum],[OptionA],[OptionAPicNum],[OptionB],[OptionBPicNum],[OptionC],[OptionCPicNum],[OptionD],[OptionDPicNum],[OptionE],[OptionEPicNum],[OptionF],[OptionFPicNum],[CreateDate],[CreateUser],[TopicNum],[CorrectAnsower],[WriteAnsower],[Type],[Remark],DaRemark,IsRight,ExamGuid";
        private string[][] ExamRecord_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"ExamID", "NVarChar", "500"},
            new string[] {"TopicTitle", "NVarChar", "500"},
            new string[] {"TopicTitlePicNum", "NVarChar", "500"},
            new string[] {"OptionA", "NVarChar", "500"},
            new string[] {"OptionAPicNum", "NVarChar", "500"},
            new string[] {"OptionB", "NVarChar", "500"},
            new string[] {"OptionBPicNum", "NVarChar", "500"},
            new string[] {"OptionC", "NVarChar", "500"},
            new string[] {"OptionCPicNum", "NVarChar", "500"},
            new string[] {"OptionD", "NVarChar", "500"},
            new string[] {"OptionDPicNum", "NVarChar", "500"},
            new string[] {"OptionE", "NVarChar", "500"},
            new string[] {"OptionEPicNum", "NVarChar", "500"},
            new string[] {"OptionF", "NVarChar", "500"},
            new string[] {"OptionFPicNum", "NVarChar", "500"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] {"CreateUser", "NVarChar", "500"},
            new string[] { "TopicNum", "Decimal", "20"},
            new string[] { "CorrectAnsower", "NVarChar", "500"},
            new string[] { "WriteAnsower", "NVarChar", "500"},
            new string[] {"Type", "NVarChar", "500"},
            new string[] { "Remark", "NVarChar", "500"},
            new string[] { "DaRemark", "NVarChar", "500"},
            new string[] { "IsRight", "Bit", "1"},
            new string[] { "ExamGuid", "NVarChar", "500"}

        };
        #endregion

        public IDataReader Get_All_ExamRecord(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamRecord_item_str + "");
            commandText.AppendLine("   FROM [ExamRecord]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamRecord_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamRecord(Entity.ExamRecord info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamRecord_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamRecord] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamRecord(Entity.ExamRecord info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamRecord_key_a, ExamRecord_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamRecord]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRecord_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamRecord(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamRecord]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRecord_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}
