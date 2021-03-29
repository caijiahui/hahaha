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

        #region ExamBank , (Ver:2.3.8) at: 2021/1/12 15:44:47
        #region Var: 
        private string[] ExamBank_key_a = { "ID" };
        private string ExamBank_item_str = "[ID],[ExamType],[TopicMajor],[TopicLevel],[TopicType],[TopicTitle],[TopicTitlePicNum],[RightKey],[Remark],[OptionA],[OptionAPicNum],[OptionB],[OptionBPicNum],[OptionC],[OptionCPicNum],[OptionD],[OptionDPicNum],[OptionE],[OptionEPicNum],[OptionF],[OptionFPicNum],[ExamSubject]";
        private string[][] ExamBank_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"ExamType", "NVarChar", "500"},
            new string[] {"TopicMajor", "NVarChar", "500"},
            new string[] {"TopicLevel", "NVarChar", "500"},
            new string[] {"TopicType", "NVarChar", "500"},
            new string[] {"TopicTitle", "NVarChar", "500"},
            new string[] {"TopicTitlePicNum", "NVarChar", "500"},
            new string[] {"RightKey", "NVarChar", "500"},
            new string[] {"Remark", "NVarChar", "500"},
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
            new string[] {"CreateUser", "NVarChar", "50"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] { "ExamSubject", "NVarChar", "500" }
        };
        #endregion

        public IDataReader Get_All_ExamBank(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamBank_item_str + "");
            commandText.AppendLine("   FROM [ExamBank]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamBank_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_ExamBank_ExamType_Rule(string TopicType, string TopicMajor, string TopicLevel,string ExamSubject)
        {
            StringBuilder commandText = new StringBuilder();
            List<DbParameter> l_parms = new List<DbParameter>();
            commandText.AppendLine(" SELECT " + ExamBank_item_str + "");
            commandText.AppendLine("   FROM [ExamBank]");
            var texts = "where TopicType=@TopicType";
            if (!string.IsNullOrEmpty(TopicLevel))
            {
                texts += " and TopicLevel=@TopicLevel";
            }
            else if (!string.IsNullOrEmpty(TopicMajor))
            {
                texts += " and TopicMajor=@TopicMajor";
            }
            else if (!string.IsNullOrEmpty(ExamSubject))
            {
                texts += " and ExamSubject like N'%" + ExamSubject + "%' ";
            }
            commandText.AppendLine(texts);
            l_parms.Add(SqlHelper.MakeInParam("@TopicType", (DbType)SqlDbType.NVarChar, 500, TopicType));
            l_parms.Add(SqlHelper.MakeInParam("@TopicMajor", (DbType)SqlDbType.NVarChar, 500, TopicMajor));
            l_parms.Add(SqlHelper.MakeInParam("@TopicLevel", (DbType)SqlDbType.NVarChar, 500, TopicLevel));
            l_parms.Add(SqlHelper.MakeInParam("@ExamSubject", (DbType)SqlDbType.NVarChar, 500, ExamSubject));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamBank(Entity.ExamBank info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamBank_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamBank] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public int Update_ExamBank(Entity.ExamBank info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamBank_key_a, ExamBank_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamBank]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamBank_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamBank(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamBank]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamBank_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion



    }
}
