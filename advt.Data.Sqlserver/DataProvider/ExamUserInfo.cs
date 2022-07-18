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
        #region ExamUserInfo , (Ver:2.3.8) at: 2021/2/5 11:25:22
        #region Var: 
        private string[] ExamUserInfo_key_a = { "ID" };
        private string ExamUserInfo_item_str = "[ID],[UserCode],[UserName],[DepartCode],[PostName],[RankName],[EntryDate],[Achievement],[TypeName],[SubjectName],[CreateUser],[CreateDate],[UpdateUser],[UpdateDate],[ReverseBuckle],[ReverseBuckleDate],[ReverseBuckleUser],ApplicationLevel,PostID,WorkPlace,isJobStatus,Quota,StopUser,StopDate,EStatus,IsEnable,LocalGrade,WorkState";
        private string[][] ExamUserInfo_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"UserCode", "NVarChar", "500"},
            new string[] {"UserName", "NVarChar", "500"},
            new string[] {"DepartCode", "NVarChar", "500"},
            new string[] {"PostName", "NVarChar", "500"},
            new string[] {"RankName", "NVarChar", "500"},
            new string[] {"EntryDate", "DateTime", "16"},
            new string[] {"Achievement", "NVarChar", "500"},
            new string[] {"TypeName", "NVarChar", "500"},
            new string[] {"SubjectName", "NVarChar", "500"},
            new string[] {"CreateUser", "NVarChar", "500"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] {"UpdateUser", "NVarChar", "500"},
            new string[] {"UpdateDate", "DateTime", "16"},
            new string[] { "ReverseBuckle", "NVarChar", "500"},
            new string[] { "ReverseBuckleDate", "DateTime", "16"},
            new string[] { "ReverseBuckleUser", "NVarChar", "500" },
            new string[] { "ApplicationLevel", "NVarChar", "500"},
            new string[] { "PostID", "NVarChar", "500"},
             new string[] { "WorkPlace", "NVarChar", "500"},
            new string[] { "isJobStatus", "Bit", "1"},
             new string[] { "Quota", "Decimal", "500"},
            new string[] { "StopUser", "NVarChar", "500"},
             new string[] { "StopDate", "DateTime", "16"},
            new string[] { "EStatus", "Bit", "1"},
            new string[] { "IsEnable", "Bit", "1"},
            new string[] { "LocalGrade", "NVarChar", "50"},
            new string[] { "WorkState", "NVarChar", "50"}



        };
        #endregion

        public IDataReader Get_All_ExamUserInfo(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamUserInfo_item_str + "");
            commandText.AppendLine("   FROM [ExamUserInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamUserInfo_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public int Insert_ElectronicUser_usercode(string usercode, string UserCostCenter, string SubjectName, string username)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("insert ExamUserInfo(UserCode,PostID,CreateDate,Quota,IsEnable,UserName,DepartCode,SubjectName,CreateUser,EStatus,TypeName)  ");
            commandText.AppendLine(" select distinct a.UserCode,a.UserJobType ,GETDATE(),b.ElectronicQuota ");
            commandText.AppendLine(" ,CONVERT(bit,0),a.UserDspName,a.UserCostCenter,b.SubjectName, '" + username + "','false',N'电子端岗位技能津贴' from advt_user_sheet a ");
            commandText.AppendLine(" inner join ExamSubject b on b.SubjectName=N'" + SubjectName + "' ");
            commandText.AppendLine(" where a.UserCode='" + usercode + "'  and a.UserCostCenter='" + UserCostCenter + "' ");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString());
        }

        public int Insert_ExamUserInfo(Entity.ExamUserInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamUserInfo_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamUserInfo] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_ElectronicUserView()
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT * from ViewExamElectronicInfo");
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }

        public int Update_ExamUserInfo(Entity.ExamUserInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamUserInfo_key_a, ExamUserInfo_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamUserInfo]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUserInfo_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamUserInfo(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamUserInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUserInfo_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Get_UpdateExamUserInfo(string UserCode, string Achievement)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            commandText.AppendLine(" update ExamUserInfo set Achievement=@Achievement where UserCode=@UserCode");
            l_parms.Add(SqlHelper.MakeInParam("@Achievement", (DbType)SqlDbType.NVarChar, 150, Achievement));
            l_parms.Add(SqlHelper.MakeInParam("@UserCode", (DbType)SqlDbType.NVarChar, 150, UserCode));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());

        }
        public int ReverseExamUserInfo(int ID, string Level, string username)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            var ApplicationLevel = Level.Substring(1, 1);
            int lev = Convert.ToInt32(ApplicationLevel) - 1;
            Level = "G" + lev;
            DateTime date = DateTime.Now;
            commandText.AppendLine(" update ExamUserInfo set ApplicationLevel=@Level,ReverseBuckleUser=@username,ReverseBuckleDate=@date  where ID=@ID");
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            l_parms.Add(SqlHelper.MakeInParam("@Level", (DbType)SqlDbType.NVarChar, 500, Level));
            l_parms.Add(SqlHelper.MakeInParam("@username", (DbType)SqlDbType.NVarChar, 500, username));
            l_parms.Add(SqlHelper.MakeInParam("@date", (DbType)SqlDbType.DateTime, 50, date));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }


        #endregion
    }
}
