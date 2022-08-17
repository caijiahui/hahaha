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
        #region advt_user_sheet , (Ver:2.3.8) at: 2021/3/18 14:33:08
        #region Var: 
        private string[] advt_user_sheet_key_a = { };
        private string advt_user_sheet_item_str = "[UserCode],[UserAccount],[UserEmail],[UserDspName],[UserCostCenter],[UserGroup],[UserJobTitle],[UserJobType]";
        private string[][] advt_user_sheet_item_prop_a =
        {
            new string[] {"UserCode", "NVarChar", "50"},
            new string[] {"UserAccount", "NVarChar", "50"},
            new string[] {"UserEmail", "NVarChar", "50"},
            new string[] {"UserDspName", "NVarChar", "50"},
            new string[] {"UserCostCenter", "NVarChar", "50"},
            new string[] {"UserGroup", "NVarChar", "50"},
            new string[] {"UserJobTitle", "NVarChar", "50"},
            new string[] { "UserJobType", "NVarChar", "50"}

        };
        #endregion

        public IDataReader Get_All_advt_user_sheet(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + advt_user_sheet_item_str + "");
            commandText.AppendLine("   FROM [advt_user_sheet]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(advt_user_sheet_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_advt_user_sheet_ElectronicUser(string sdata,string subject)
        {
            StringBuilder commandText = new StringBuilder();
            var cmd = string.Empty;
            if (!string.IsNullOrEmpty(subject))
            {
                cmd = "select UserCode from ExamUserInfo  where IsEnable=0 and TypeName=N'电子端岗位技能津贴' and SubjectName=N'"+ subject + "'";
            }
            else
            {
                cmd = "select UserCode from ExamUserInfo  where IsEnable = 0 and TypeName = N'电子端岗位技能津贴'";
            }
            commandText.AppendLine(" select a.* from advt_user_sheet a where a.UserCode not in ( "+cmd+") and (a.UserCode = '" + sdata+ "'  or a.UserDspName='" + sdata + "' or a.UserCostCenter='" + sdata + "')");
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        
        public int Insert_advt_user_sheet(Entity.advt_user_sheet info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(advt_user_sheet_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [advt_user_sheet] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        //Get_advt_user_sheet_UserJobTitle
        public IDataReader Get_advt_user_sheet_UserJobTitle(string UserCode)
        {
            StringBuilder commandText = new StringBuilder();
            List<DbParameter> l_parms = new List<DbParameter>();
            commandText.AppendLine(" SELECT " + advt_user_sheet_item_str + "");
            commandText.AppendLine("   FROM [advt_user_sheet] where  UserCode=@UserCode and (UserJobTitle like N'%课长%' or UserJobTitle like N'%副课长%' or UserJobTitle like N'%部级主管%') ");
            //List<DbParameter> l_parms = SqlHelper.Get_List_Params(advt_user_sheet_item_prop_a, UserAccount);
            l_parms.Add(SqlHelper.MakeInParam("@UserCode", (DbType)SqlDbType.NVarChar, 50, UserCode));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms);
        }
        #endregion
    }
}
