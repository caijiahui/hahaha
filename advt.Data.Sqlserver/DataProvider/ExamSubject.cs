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
        #region ExamSubject , (Ver:2.3.8) at: 2021/1/7 16:05:32
        #region Var: 
        private string[] ExamSubject_key_a = { "ID" };
        private string ExamSubject_item_str = "[ID],[SubjectName],[CreateUser],[CreateDate],[TypeName],[ExamRuleName],HCLimit,IsSysPractice,IsProAssess,DepartCode,MajorQuota,ElectronicQuota,SkillsAllowance,GradePosition,IsAddAllowance,PostQuota,ShangGangName";
        private string[][] ExamSubject_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"SubjectName", "NVarChar", "2147483646"},
            new string[] {"CreateUser", "NVarChar", "2147483646"},
            new string[] {"CreateDate", "DateTime", "16"},
            new string[] { "TypeName", "NVarChar", "2147483646"},
            new string[] { "ExamRuleName", "NVarChar", "2147483646"},
            new string[] { "HCLimit", "Int", "4"},
            new string[] { "IsSysPractice", "Bit", "1"},
            new string[] { "IsProAssess", "Bit", "1"},
            new string[] { "DepartCode", "NVarChar", "50"},
            new string[] { "ElectronicQuota", "Int", "4"},
            new string[] { "MajorQuota", "Int", "4"},
            new string[] { "SkillsAllowance", "Int", "4"},
            new string[] { "GradePosition", "Int", "4"},
            new string[] { "IsAddAllowance", "Bit", "4"},
            new string[] { "PostQuota", "Int", "4"},
            new string[] { "ShangGangName", "NVarChar", "50"}
              
        };
        #endregion

        public IDataReader Get_All_ExamSubject(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamSubject_item_str + "");
            commandText.AppendLine("   FROM [ExamSubject]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamSubject_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Insert_ExamSubject(Entity.ExamSubject info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamSubject_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamSubject] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamSubject(Entity.ExamSubject info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamSubject_key_a, ExamSubject_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamSubject]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamSubject_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamSubject(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamSubject]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamSubject_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public IDataReader GetSubjectList(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT * from ExamType");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamSubject_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_ExamSubjectInfo(string SubjectName,string TypeName)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from ExamSubject where SubjectName=@SubjectName and TypeName=@TypeName");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@SubjectName", (DbType)SqlDbType.NVarChar, 150, SubjectName));
            l_parms.Add(SqlHelper.MakeInParam("@TypeName", (DbType)SqlDbType.NVarChar, 150, TypeName));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public IDataReader Get_All_ExamGetSubject(string ExamType,string SubjectName)
        {
            StringBuilder commandText = new StringBuilder();
            var text = "";
            commandText.AppendLine(" SELECT " + ExamSubject_item_str + "");
            commandText.AppendLine("   FROM [ExamSubject] ");
            if (!string.IsNullOrEmpty(ExamType)&& (!string.IsNullOrEmpty(SubjectName)))
            {
                text = " where TypeName like N'%" + ExamType + "%' and  SubjectName like N'%" + SubjectName + "%' ";
            }
            else if (!string.IsNullOrEmpty(ExamType))
            {
                text = " where TypeName like N'%" + ExamType + "%' ";
            }
           else if (!string.IsNullOrEmpty(SubjectName))
            {
                text = " where SubjectName like N'%" + SubjectName + "%' ";
            }
            commandText.AppendLine(text);
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ExamType", (DbType)SqlDbType.NVarChar, 150, ExamType));
            l_parms.Add(SqlHelper.MakeInParam("@SubjectName", (DbType)SqlDbType.NVarChar, 150, SubjectName));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_ExamGetByDepart(string Depart)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("select a.SubjectName,typename=case when b.counts is null then a.SubjectName+N'可报'+convert(nvarchar, a.HCLimit)+N'人' else "+
" a.SubjectName + N'可报' + convert(nvarchar, a.HCLimit - b.counts) + N'人' end from ExamSubject a left join (" +
                "  select b.SubjectName,counts= COUNT(*) from (select * from ExamSubject a where  " + Depart + ")a " +
                "  inner join ( select * from ExamUserDetailInfo where TypeName=N'电子端岗位技能津贴' and IsStop=0 and IsExam='false' " +
                "  union all select * from ExamUserDetailInfo where TypeName=N'电子端岗位技能津贴' and IsStop=0 and IsExam='true' and IsExamPass=1) " +
                "  b on a.SubjectName = b.SubjectName group by b.SubjectName ) b on a.SubjectName = b.SubjectName  where   " + Depart 
                ) ; 
            
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        
        #endregion
    }
}
