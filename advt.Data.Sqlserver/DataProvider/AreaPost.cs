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
        private string[] AreaPost_key_a = { "ID" };
        private string AreaPost_item_str = "[ID], [Area], [Dept], [PostName], [PostID]";
        private string[][] AreaPost_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] { "Area", "NVarChar", "50"},
            new string[] { "Dept", "NVarChar", "50"},
            new string[] { "PostName", "NVarChar", "50"},
            new string[] { "PostID", "NVarChar", "50"}
        };
        public IDataReader Get_All_AreaPost(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + AreaPost_item_str + "");
            commandText.AppendLine("   FROM [AreaPost]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(AreaPost_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_AreaPostArea()
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("select distinct Area from AreaPost ");
            List<DbParameter> l_parms = new List<DbParameter>();
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_AreaPostDept(string model)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("select distinct Dept from AreaPost where Area=@Area");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@Area", (DbType)SqlDbType.NVarChar, 150, model));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_AreaPostName(string model)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("select distinct PostName from AreaPost where Dept=@Dept");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@Dept", (DbType)SqlDbType.NVarChar, 150, model));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
    }
}
