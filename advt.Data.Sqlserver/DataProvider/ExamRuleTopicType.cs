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

        #region ExamRuleTopicType , (Ver:2.3.8) at: 2021/1/12 15:49:07
        #region Var: 
        private string[] ExamRuleTopicType_key_a = { "ID" };
        private string ExamRuleTopicType_item_str = "[ID],[TopicMajor],[TopicLevel],[TopicType],[TopicNum],[TopicScore],[RuleId]";
        private string[][] ExamRuleTopicType_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"TopicMajor", "NVarChar", "500"},
            new string[] {"TopicLevel", "NVarChar", "500"},
            new string[] {"TopicType", "NVarChar", "500"},
            new string[] {"TopicNum", "NVarChar", "500"},
            new string[] {"TopicScore", "Decimal", "20"},
            new string[] {"RuleId", "Int", "4"}
        };
        #endregion

        public IDataReader Get_All_ExamRuleTopicType(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamRuleTopicType_item_str + "");
            commandText.AppendLine("   FROM [ExamRuleTopicType]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamRuleTopicType_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamRuleTopicType(Entity.ExamRuleTopicType info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamRuleTopicType_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamRuleTopicType] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamRuleTopicType(Entity.ExamRuleTopicType info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamRuleTopicType_key_a, ExamRuleTopicType_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamRuleTopicType]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRuleTopicType_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamRuleTopicType(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamRuleTopicType]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRuleTopicType_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_ExamRuleTopic(string typnid)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from ExamRuleTopicType where RuleId=@typnid");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@typnid", (DbType)SqlDbType.NVarChar, 150, typnid));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_ExamRuleInfo(string TopicLevel, string TopicMajor, string TopicType,int id)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from ExamRuleTopicType where TopicMajor=@TopicMajor and TopicLevel =@TopicLevel  and RuleId =@id and TopicType =@TopicType");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@TopicLevel", (DbType)SqlDbType.NVarChar, 150, TopicLevel));
            l_parms.Add(SqlHelper.MakeInParam("@TopicMajor", (DbType)SqlDbType.NVarChar, 150, TopicMajor));
            l_parms.Add(SqlHelper.MakeInParam("@TopicType", (DbType)SqlDbType.NVarChar, 150, TopicType));
            l_parms.Add(SqlHelper.MakeInParam("@id", (DbType)SqlDbType.NVarChar, 150, id));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        
        #endregion



    }
}
