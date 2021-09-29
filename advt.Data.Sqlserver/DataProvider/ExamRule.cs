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

        #region ExamRule , (Ver:2.3.8) at: 2021/1/9 14:56:07
        #region Var: 
        private string[] ExamRule_key_a = { "ID" };
        private string ExamRule_item_str = "[ID],[TypeName],[TotalScore],[TotalTime],[TotalSubject],[PassScore],[IsRead],[IsRepeat],[StartDeac],[EndDesc],[CreateUser],[CreateDate],[RuleName],[SubjectName],IsQuestion,PassPracticeScore,RegionalPlace,DepartCode,PostName";
        private string[][] ExamRule_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"TypeName", "NVarChar", "50"},
            new string[] {"TotalScore", "Decimal", "20"},
            new string[] {"TotalTime", "Decimal", "20"},
            new string[] {"TotalSubject", "Int", "4"},
            new string[] {"PassScore", "Decimal", "20"},
            new string[] {"IsRead", "Bit", "1"},
            new string[] {"IsRepeat", "Bit", "1"},
            new string[] {"StartDeac", "NVarChar", "500"},
            new string[] {"EndDesc", "NVarChar", "500"},
            new string[] {"CreateUser", "NVarChar", "100"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] {"RuleName", "NVarChar", "150"},
            new string[] {"SubjectName", "NVarChar", "150"},
            new string[] { "IsQuestion", "Bit", "1"},
            new string[] { "PassPracticeScore", "Decimal", "20"},
            new string[] { "RegionalPlace", "NVarChar", "150"},
            new string[] { "DepartCode", "NVarChar", "150"},
            new string[] { "PostName", "NVarChar", "150"}
        };
       
        #endregion

        public IDataReader Get_All_ExamRule(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamRule_item_str + "");
            commandText.AppendLine("   FROM [ExamRule]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamRule_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamRule(Entity.ExamRule info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamRule_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamRule] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamRule(Entity.ExamRule info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamRule_key_a, ExamRule_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamRule]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRule_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamRule(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamRule]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamRule_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public IDataReader GetSubjectList(string typname)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT * from ExamSubject where TypeName=@typname");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@typname", (DbType)SqlDbType.NVarChar, 150, typname));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader GetTopicInfo(string typname)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select  [TopicMajor],[TopicLevel],[TopicType],Bcount= count(*) from[dbo].[ExamBank] where ExamType =@typname group by [TopicMajor],[TopicLevel],[TopicType]");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@typname", (DbType)SqlDbType.NVarChar, 150, typname));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader GetTRuleSubjectInfo(string model)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select  [TopicMajor],[TopicLevel],[TopicType],Bcount= count(*) from[dbo].[ExamBank] where ExamSubject like N'%" + @model+"%' group by [TopicMajor],[TopicLevel],[TopicType]");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@model", (DbType)SqlDbType.NVarChar, 150, model));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_ExamRuleInfo(string RuleName)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from ExamRule where RuleName=@RuleName");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@RuleName", (DbType)SqlDbType.NVarChar, 150, RuleName));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());

        }
        public IDataReader Get_All_ExamGetRuleName(string ExamType, string SubjectName, string RuleName)
        {
            StringBuilder commandText = new StringBuilder();
            var texts = "";
            commandText.AppendLine(" SELECT " + ExamRule_item_str + "");
            commandText.AppendLine("   FROM [ExamRule] where 1=1 ");
            if (!string.IsNullOrEmpty(ExamType))
            {
                texts += " and TypeName like  N'%" + ExamType + "%'";
            }
            if (!string.IsNullOrEmpty(SubjectName))
            {
                texts += " and SubjectName like  N'%" + SubjectName + "%'";
            }
            if (!string.IsNullOrEmpty(RuleName))
            {
                texts += " and RuleName like  N'%" + RuleName + "%'";
            }          
           
            commandText.AppendLine(texts);
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ExamType", (DbType)SqlDbType.NVarChar, 150, ExamType));
            l_parms.Add(SqlHelper.MakeInParam("@SubjectName", (DbType)SqlDbType.NVarChar, 150, SubjectName));
            l_parms.Add(SqlHelper.MakeInParam("@RuleName", (DbType)SqlDbType.NVarChar, 150, RuleName));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());

        }       
        #endregion



    }
}
