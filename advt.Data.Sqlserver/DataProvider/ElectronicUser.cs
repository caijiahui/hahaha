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

        #region ElectronicUser , (Ver:2.3.8) at: 2022/1/27 15:46:50
        #region Var: 
        private string[] ElectronicUser_key_a = { "ID" };
        private string ElectronicUser_item_str = "[UserCode],[ElectronicPost],[CreateDate],[ElectronicQuota],[IsEnable],[ID],[UserName],[Department],[SubjectName],[StopDate],[StopUser],[CreateUser]";
        private string[][] ElectronicUser_item_prop_a =
        {
            new string[] {"UserCode", "NVarChar", "50"},
            new string[] {"ElectronicPost", "NVarChar", "50"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] {"ElectronicQuota", "Decimal", "20"},
            new string[] {"IsEnable", "Bit", "1"},
            new string[] {"ID", "Int", "4"},
            new string[] {"UserName", "NVarChar", "50"},
            new string[] {"Department", "NVarChar", "50"},
            new string[] {"SubjectName", "NVarChar", "50"},
            new string[] {"StopDate", "DateTime", "16"},
            new string[] {"StopUser", "NVarChar", "16"},
             new string[] {"CreateUser", "NVarChar", "16"}
        };
        #endregion

        public IDataReader Get_All_ElectronicUser(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ElectronicUser_item_str + "");
            commandText.AppendLine("   FROM [ElectronicUser]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ElectronicUser_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_ElectronicUserView()
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT * from ViewExamElectronicInfo" );
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        
            public int Insert_ElectronicUser_usercode(string usercode,string UserCostCenter,string SubjectName,string username)
            {
                StringBuilder commandText = new StringBuilder();
                commandText.AppendLine("insert ElectronicUser(UserCode,ElectronicPost,CreateDate,ElectronicQuota,IsEnable,UserName,Department,SubjectName,CreateUser) ");
                commandText.AppendLine(" select distinct a.UserCode,a.UserJobType ,GETDATE(),b.ElectronicQuota ");
                commandText.AppendLine(" ,CONVERT(bit,0),a.UserDspName,a.UserCostCenter,b.SubjectName, '"+username+"' from advt_user_sheet a ");
                commandText.AppendLine(" inner join ExamSubject b on b.SubjectName=N'"+ SubjectName + "' ");
                commandText.AppendLine(" where a.UserCode='"+ usercode + "'  and a.UserCostCenter='"+ UserCostCenter + "' ");
                return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString());
            }
        public int Insert_ElectronicUser(Entity.ElectronicUser info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ElectronicUser_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ElectronicUser] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ElectronicUser(Entity.ElectronicUser info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ElectronicUser_key_a, ElectronicUser_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ElectronicUser]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ElectronicUser_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ElectronicUser(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ElectronicUser]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ElectronicUser_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        #endregion
    }
}