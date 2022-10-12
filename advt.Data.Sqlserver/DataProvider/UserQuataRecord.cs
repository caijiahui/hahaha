using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data.SqlServer
{
    public partial class DataProvider : advt.Data.IDataProvider
    {
        private string[] UserQuataRecord_key_a = { "ID" };
        private string UserQuataRecord_item_str = "[ID],[UserCode],[UserName],[RuleName],[SubjectName],[TypeName],ElectronicQuota,MajorQuota,SkillsAllowance,GradePosition,PostQuota,TotalQuota,CreateName,CreateDate";
        private string[][] UserQuataRecord_item_prop_a =
        {
           new string[] {"ID", "Int", "4"},
            new string[] {"UserCode", "NVarChar", "500"},
            new string[] {"UserName", "NVarChar", "500"},            
            new string[] {"RuleName", "NVarChar", "500"},
            new string[] {"SubjectName", "NVarChar", "500"},
            new string[] {"TypeName", "NVarChar", "500"},           
            new string[] { "CreateName", "NVarChar", "500"},
            new string[] { "CreateDate", "DateTime", "16"},           
            new string[] { "ElectronicQuota", "Int", "4"},
            new string[] { "MajorQuota", "Int", "4"},
            new string[] { "SkillsAllowance", "Int", "4"},
            new string[] { "GradePosition", "Int", "4"},
            new string[] { "PostQuota", "Int", "4"},
            new string[] { "TotalQuota", "Int", "4"}
        };
        public IDataReader Get_All_UserQuataRecord(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + UserQuataRecord_item_str + "");
            commandText.AppendLine("   FROM [UserQuataRecord]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(UserQuataRecord_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_UserQuataRecord(Entity.UserQuataRecord info, string[] Include, string[] Exclude)
        {
            try
            {
                List<DbParameter> l_parms = new List<DbParameter>();
                StringBuilder commandText = new StringBuilder();
                string item_name = string.Empty;
                string item_value = string.Empty;
                SqlHelper.Get_Inserte_Set(UserQuataRecord_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
                commandText.AppendLine("INSERT INTO [UserQuataRecord] (" + item_name + ") VALUES (" + item_value + ")");
                return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
