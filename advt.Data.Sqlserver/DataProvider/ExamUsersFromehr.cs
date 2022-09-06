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

        #region ExamUsersFromehr , (Ver:2.3.8) at: 2021/5/11 11:39:36
        #region Var: 
        private string[] ExamUsersFromehr_key_a = { "UserCode" };
        private string ExamUsersFromehr_item_str = "[UserCode],[UserName],[UserDept],[UserDeptName],[JoinDate],[JobCode],[JobType],[Gender],[Birthdate],[CommpanyEmail],[State],[Rank],[DLIDL],[Marriage],[EMPLOYEECHARID],[CERTIFICATENUMBER],[OrgName],[Password],[LoginFlag],[EamilUsername]";
        private string[][] ExamUsersFromehr_item_prop_a =
        {
            new string[] {"UserCode", "NVarChar", "30"},
            new string[] {"UserName", "NVarChar", "30"},
            new string[] {"UserDept", "NVarChar", "30"},
            new string[] {"UserDeptName", "NVarChar", "50"},
            new string[] {"JoinDate", "DateTime", "16"},
            new string[] {"JobCode", "NVarChar", "100"},
            new string[] {"JobType", "NVarChar", "100"},
            new string[] {"Gender", "NVarChar", "100"},
            new string[] {"Birthdate", "DateTime", "16"},
            new string[] {"CommpanyEmail", "NVarChar", "100"},
            new string[] {"State", "NVarChar", "20"},
            new string[] {"Rank", "NChar", "20"},
            new string[] {"DLIDL", "Int", "4"},
            new string[] {"Marriage", "Int", "4"},
            new string[] {"EMPLOYEECHARID", "NVarChar", "10"},
            new string[] {"CERTIFICATENUMBER", "NVarChar", "50"},
            new string[] {"OrgName", "NVarChar", "100"},
            new string[] {"Password", "NChar", "32"},
            new string[] {"LoginFlag", "VarChar", "50"},
            new string[] {"EamilUsername", "NVarChar", "30"}
        };
        #endregion

        public IDataReader Get_All_ExamUsersFromehr(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamUsersFromehr_item_str + "");
            commandText.AppendLine("   FROM [ExamUsersFromehr]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            var dd = objparams.ToString();
            if (dd!= "{ }")
            {
                commandText.AppendLine("  and [State]!=N'¿Î÷∞'");
            }
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamUsersFromehr_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        //Get_All_SuperUser
        public IDataReader Get_All_SuperUser(string typename, string subject, string sData, string UserName)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from ExamUsersFromehr a inner join advt_user_sheet b on a.UserCode = b.UserCode and b.UserCostCenter = a.UserDept where a.OrgName in ( select OrgName from ExamUsersFromehr where CommpanyEmail like '%" + UserName + "%' ) ");
            commandText.AppendLine("   and  (a.UserCode='"+ sData + "' or a.UserDept='"+sData+"' or a.UserName='"+sData+ "') and a.UserCode not in ( ");
            commandText.AppendLine("   select b.UserCode from ExamUserDetailInfo b where b.TypeName=N'"+ typename + "' and b.SubjectName=N'"+ subject + "' ");
            commandText.AppendLine("   and  IsExam='false' and IsStop='false' ) and a.[State]!='¿Î÷∞'");
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        public int Insert_ExamUsersFromehr(Entity.ExamUsersFromehr info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamUsersFromehr_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamUsersFromehr] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamUsersFromehr(Entity.ExamUsersFromehr info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamUsersFromehr_key_a, ExamUsersFromehr_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamUsersFromehr]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUsersFromehr_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamUsersFromehr(string UserCode)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamUsersFromehr]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUsersFromehr_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@UserCode", (DbType)SqlDbType.NVarChar, 30, UserCode));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}