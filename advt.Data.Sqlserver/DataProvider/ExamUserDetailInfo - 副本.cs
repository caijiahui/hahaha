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
        #region ExamUserDetailInfo , (Ver:2.3.8) at: 2021/3/4 9:31:04
        #region Var: 
        private string[] ExamUserDetailInfo_key_a = { "ID" };
        private string ExamUserDetailInfo_item_str = "[ID],[UserCode],[UserName],[DepartCode],[PostName],[RankName],[SkillName],[EntryDate],[Achievement],[ExamDate],[ExamScore],[PracticeScore],[PlanExamDate],[ExamPlace],[ExamStatus],[IsReview],[RuleName],[SubjectName],[TypeName],[ApplyLevel],[HighestLevel],[IsAchievement],[IsStop],[IsExam],[HrCreateUser],[HrCreateDate],[DirectorCreateUser],[DirectorCreateDate],[HrCheckCreateUser],[HrCheckCreateDate],[StopCreateUser],[StopCreateDate],UserExamDate,IsUserExam,ExamStatue,IsExamPass,WorkPlace,PostID,OrgName,SignType,ElectronicQuota,MajorQuota,SkillsAllowance,GradePosition,PostQuota,TotalQuota,State,Type,IsStartExam,StartExamUser,StartExamDate,ExamKind";

        private string[][] ExamUserDetailInfo_item_prop_a =
        {
            new string[] {"ID", "Int", "4"},
            new string[] {"UserCode", "NVarChar", "500"},
            new string[] {"UserName", "NVarChar", "500"},
            new string[] {"DepartCode", "NVarChar", "500"},
            new string[] {"PostName", "NVarChar", "500"},
            new string[] {"RankName", "NVarChar", "500"},
            new string[] {"SkillName", "NVarChar", "500"},
            new string[] {"EntryDate", "DateTime", "16"},
            new string[] {"Achievement", "NVarChar", "500"},
            new string[] {"ExamDate", "DateTime", "16"},
            new string[] {"ExamScore", "Decimal", "20"},
            new string[] {"PracticeScore", "Decimal", "20"},
            new string[] {"PlanExamDate", "DateTime", "16"},
            new string[] {"ExamPlace", "NVarChar", "500"},
            new string[] {"ExamStatus", "NVarChar", "500"},
            new string[] {"IsReview", "Bit", "1"},
            new string[] {"RuleName", "NVarChar", "500"},
            new string[] {"SubjectName", "NVarChar", "500"},
            new string[] {"TypeName", "NVarChar", "500"},
            new string[] {"ApplyLevel", "NVarChar", "500"},
            new string[] {"HighestLevel", "NVarChar", "500"},
            new string[] {"IsAchievement", "Bit", "1"},
            new string[] {"IsStop", "Bit", "1"},
            new string[] {"IsExam", "NVarChar", "500"},
            new string[] { "HrCreateUser", "NVarChar", "500"},
            new string[] { "HrCreateDate", "DateTime", "16"},
            new string[] { "DirectorCreateUser", "NVarChar", "500"},
            new string[] { "DirectorCreateDate", "DateTime", "16"},
            new string[] { "HrCheckCreateUser", "NVarChar", "500"},
            new string[] { "HrCheckCreateDate", "DateTime", "16"},
            new string[] { "StopCreateUser", "NVarChar", "500"},
            new string[] { "StopCreateDate", "DateTime", "16"},
            new string[] { "UserExamDate", "DateTime", "16"},
            new string[] { "IsUserExam", "NVarChar", "500"},
            new string[] { "ExamStatue", "Bit", "1"},
            new string[] { "IsExamPass", "Bit", "1"},
            new string[] { "WorkPlace", "NVarChar", "50"},
            new string[] { "PostID", "NVarChar", "50"},
            new string[] { "OrgName", "NVarChar", "50"},
            new string[] { "SignType", "NVarChar", "50"},
            new string[] { "ElectronicQuota", "Int", "4"},
            new string[] { "MajorQuota", "Int", "4"},
            new string[] { "SkillsAllowance", "Int", "4"},
            new string[] { "GradePosition", "Int", "4"},
            new string[] { "PostQuota", "Int", "4"},
            new string[] { "TotalQuota", "Int", "4"},
            new string[] { "State", "NVarChar", "10"},
            new string[] { "Type", "NVarChar", "50"},
            new string[] { "IsStartExam", "Bit", "1"},
            new string[] { "StartExamUser", "NVarChar", "50"},
            new string[] { "StartExamDate", "DateTime", "16"},
            new string[] { "ExamKind", "NVarChar", "50"}
        };
        #endregion

        public IDataReader Get_All_ExamUserDetailInfo(object objparams)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamUserDetailInfo_item_str + "");
            commandText.AppendLine("   FROM [ExamUserDetailInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Obj(objparams));
            List<DbParameter> l_parms = SqlHelper.Get_List_Params(ExamUserDetailInfo_item_prop_a, objparams);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }
        public IDataReader Get_All_ExamUserDetailInfo(string Typename, string UserCode, string SubjectName, string DepartCode)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" SELECT " + ExamUserDetailInfo_item_str + "");
            commandText.AppendLine("  from ExamUserDetailInfo where ExamStatus='Signup' and IsStop='false' and IsExam='false' "); 
            var text = "";
            if (!string.IsNullOrEmpty(Typename))
            {
                text += " and Typename like N'%" + Typename + "%'";
            }
            if (!string.IsNullOrEmpty(UserCode))
            {
                text += " and UserCode like N'%" + UserCode + "%'";
            }
            if (!string.IsNullOrEmpty(SubjectName))
            {
                text += " and SubjectName like N'%" + SubjectName + "%'";
            }
            if (!string.IsNullOrEmpty(DepartCode))
            {
                text += " and DepartCode like N'%" + DepartCode + "%'";
            }
            commandText.AppendLine(text);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        public IDataReader Get_All_ExamUserCheckDetail(string Typename, string UserCode, string SubjectName, string DepartCode)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine("  select * from ExamUserDetailInfo where ExamStatus='HrCheck' and IsStop='false' and IsExam='false' ");
            var text = "";
            if (!string.IsNullOrEmpty(Typename))
            {
                text += " and Typename like N'%" + Typename + "%'";
            }
            if (!string.IsNullOrEmpty(UserCode))
            {
                text += " and UserCode like N'%" + UserCode + "%'";
            }
            if (!string.IsNullOrEmpty(SubjectName))
            {
                text += " and SubjectName like N'%" + SubjectName + "%'";
            }
            if (!string.IsNullOrEmpty(DepartCode))
            {
                text += " and DepartCode like N'%" + DepartCode + "%'";
            }
            commandText.AppendLine(text);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }


        public int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string item_name = string.Empty;
            string item_value = string.Empty;
            SqlHelper.Get_Inserte_Set(ExamUserDetailInfo_item_prop_a, Include, Exclude, info, ref item_name, ref item_value, ref l_parms);
            commandText.AppendLine("INSERT INTO [ExamUserDetailInfo] (" + item_name + ") VALUES (" + item_value + ")");
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude)
        {
            List<DbParameter> l_parms = new List<DbParameter>();
            StringBuilder commandText = new StringBuilder();
            string set_str = string.Empty;
            SqlHelper.Get_Update_Set(ExamUserDetailInfo_key_a, ExamUserDetailInfo_item_prop_a, Include, Exclude, info, ref set_str, ref l_parms);
            commandText.AppendLine(" UPDATE [ExamUserDetailInfo]");
            commandText.AppendLine("   SET " + set_str);
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUserDetailInfo_key_a));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public int Delete_ExamUserDetailInfo(int ID)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" DELETE FROM [ExamUserDetailInfo]");
            commandText.AppendLine("   " + SqlHelper.Get_Where_Str(ExamUserDetailInfo_key_a));
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@ID", (DbType)SqlDbType.Int, 4, ID));
            return DbHelper.PE.ExecuteNonQuery(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        public IDataReader Get_All_ExamUserInfo(DateTime date)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select *  from ExamUserDetailInfo where convert(char(10), UserExamDate,120)=@date and IsExam='true'");
            List<DbParameter> l_parms = new List<DbParameter>();
            l_parms.Add(SqlHelper.MakeInParam("@date", (DbType)SqlDbType.DateTime, 150, date));
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString(), l_parms.ToArray());
        }

        //Get_Super_UserAduitInfo
        public IDataReader Get_Super_UserAduitInfo(string ExamStatus, string username, string typename)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from ExamType a inner join ExamUserDetailInfo b on a.TypeName = b.TypeName ");
            commandText.AppendLine(" where SuperAdmin='" + username + "' and ExamStatus='" + ExamStatus + "' and IsStop=0   ");
            commandText.AppendLine(" and b.WorkPlace in (select OrgName from ExamUsersFromehr where CommpanyEmail like '%" + username + "%' ) ");
            if (!string.IsNullOrEmpty(typename))
            {
                commandText.AppendLine(" and a.TypeName=N'" + typename + "' ");
            }
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }



        public IDataReader Get_All_ExamUserGetDetailInfo(string UserCode, string SubjectName, string date, string DepartCode)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select *  from ExamUserDetailInfo ");
            List<DbParameter> l_parms = new List<DbParameter>();
            var texts = " where IsExam='true' and IsStop=0 and ExamStatus='HrCheck' ";
            if (!string.IsNullOrEmpty(UserCode))
            {
                texts += " and UserCode like N'%" + UserCode + "%' ";
            }
            if (!string.IsNullOrEmpty(SubjectName))
            {
                texts += " and SubjectName like N'%" + SubjectName + "%' ";
            }
            if (!string.IsNullOrEmpty(date))
            {
                DateTime dates = Convert.ToDateTime(date);
                texts += " and cast(convert(varchar(10),ExamDate,120)+' 00:00:00' as datetime)= '" + dates + "'";
            }
            if (!string.IsNullOrEmpty(DepartCode))
            {
                texts += " and DepartCode like N'%" + DepartCode + "%' ";
            }
            commandText.AppendLine(texts);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }

        public IDataReader Get_ExamUserAuditInfo(string ExamStatus, string UserCode, string typename)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select distinct c.* from [advt_user_sheet] a  inner join (select * from [advt_user_sheet] where UserCode='" + UserCode + "'" +
                 " and (UserJobTitle like N'%课长%' or UserJobTitle like N'%副课长%' or UserJobTitle like N'%部级主管%')) b" +
                 " on a.UserCostCenter=b.UserCostCenter inner join ExamUserDetailInfo c on c.UserCode = a.UserCode where   ExamStatus='" + ExamStatus + "' and IsStop=0" +
                " and a.UserCostCenter = c.DepartCode ");
            if (!string.IsNullOrEmpty(typename))
            {
                commandText.AppendLine("and c.TypeName = N'" + typename + "'");
            }
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        public IDataReader Get_All_UserCelarInfo(string ExamStatus, string UserCode, string typename)
        {
            StringBuilder commandText = new StringBuilder();
            if (typename != "电子端岗位技能津贴")
            {
                commandText.AppendLine(" select distinct  c.[ID], c.[UserCode], c.[UserName], d.[DepartCode], c.[PostName], c.[RankName], c.[SkillName], c.[EntryDate], c.[Achievement], c.[ExamDate], c.[ExamScore], c.[PracticeScore], c.[PlanExamDate], c.[ExamPlace], c.[ExamStatus], c.[IsReview],\r\nc.[RuleName], c.[SubjectName], c.[TypeName], c.[ApplyLevel], c.[HighestLevel], c.[IsAchievement], c.[IsStop],  c.[IsExam],\r\n c.[UserExamDate], c.[IsUserExam], c.[ExamStatue], c.[IsExamPass], c.[WorkPlace], c.[PostID], c.[OrgName], c.[ElectronicQuota], c.[MajorQuota], c.[SkillsAllowance], c.[GradePosition], c.[PostQuota], c.[TotalQuota]  from [advt_user_sheet] a   inner join(select * from[advt_user_sheet] where UserCode ='" + UserCode + "'" +
                " and(UserJobTitle like N'%课长%' or UserJobTitle like N'%副课长%' or UserJobTitle like N'%部级主管%')) b on a.UserCostCenter = b.UserCostCenter inner join ExamUserInfo d on b.UserCostCenter=d.DepartCode " +
                "inner join(select ROW_NUMBER() over(partition by usercode order by examdate desc) ssd, * from ExamUserDetailInfo where  IsExam = 'true'   and TypeName!=N'Board技能等级考试' and SubjectName is not null) c on c.UserCode = a.UserCode where   ExamStatus = '" + ExamStatus + "'  and State in (N'试用',N'正式')  and c.TypeName=d.TypeName  and ssd = 1 ");
                if (!string.IsNullOrEmpty(typename))
                {
                    commandText.AppendLine("and c.TypeName = N'" + typename + "'");
                }
                commandText.AppendLine(" order by ExamDate desc");
            }
            else
            {
                commandText.AppendLine(" select distinct c.* from [advt_user_sheet] a   inner join(select * from[advt_user_sheet] where UserCode ='" + UserCode + "' and(UserJobTitle like N'%课长%' or " +
                    "UserJobTitle like N'%副课长%' or UserJobTitle like N'%部级主管%')) b  on a.UserCostCenter" +
                    " = b.UserCostCenter inner join( select * from [U_Cancel_UserInfo]) c " +
                    "on c.UserCode = a.UserCode where  a.UserCostCenter = c.DepartCode" +
                    " order by ExamDate desc");
            }
           
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }

        public IDataReader Get_All_PostCanSignUser(string UserCode)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select distinct TypeName=N'电子端岗位技能津贴', UserCode=a.UserCode,UserName=a.UserName,DepartCode=a.Department,SubjectName=a.SubjectName from ElectronicUser a " +
                " inner join (select * from [advt_user_sheet] where UserCode='" + UserCode + "'  and(UserJobTitle like N'%课长%' or UserJobTitle like N'%副课长%' or UserJobTitle like N'%部级主管%')) b   " +
              "   on a.Department = b.UserCostCenter where a.UserCode not in ( select a.UserCode from ElectronicUser a inner join ExamUserDetailInfo b on a.SubjectName = b.SubjectName and b.IsStop=0 ) ");
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        public IDataReader Get_All_ExamUserALLDetailInfo(string UserCode, string SubjectName, string TypeName, string OrgName, string DepartCode)
        {
            string sql = string.Empty;
            StringBuilder commandText = new StringBuilder();
            List<DbParameter> l_parms = new List<DbParameter>();
            if (string.IsNullOrEmpty(UserCode) && string.IsNullOrEmpty(TypeName) && string.IsNullOrEmpty(SubjectName) && string.IsNullOrEmpty(OrgName) && string.IsNullOrEmpty(DepartCode))
            {
                commandText.AppendLine(" select  top 50 *  from ExamUserDetailInfo ");
            }
            else
            {
                commandText.AppendLine(" select *  from ExamUserDetailInfo ");
            }

            sql = " where IsStop=0 and ExamStatus='HrCheck' and IsExam='true' ";
            if (!string.IsNullOrEmpty(UserCode) && UserCode != "undefined")
            {
                sql += " and UserCode like N'%" + UserCode + "%' ";
            }
            if (!string.IsNullOrEmpty(TypeName) && TypeName != "undefined")
            {
                sql += " and TypeName like N'%" + TypeName + "%' ";
            }
            if (!string.IsNullOrEmpty(SubjectName) && SubjectName != "undefined")
            {
                sql += " and SubjectName like N'%" + SubjectName + "%' ";
            }
            if (!string.IsNullOrEmpty(OrgName) && OrgName != "undefined")
            {
                sql += " and OrgName like N'%" + OrgName + "%' ";
            }
            if (!string.IsNullOrEmpty(DepartCode) && DepartCode != "undefined")
            {
                sql += " and DepartCode like N'%" + DepartCode + "%' ";
            }
            sql += " order by ExamDate desc";
            commandText.AppendLine(sql);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }

        public IDataReader Get_UserInfo(string UserCode, DateTime? ExamDate)
        {
            string sql = string.Empty;
            StringBuilder commandText = new StringBuilder();
            List<DbParameter> l_parms = new List<DbParameter>();
            if (string.IsNullOrEmpty(UserCode))
            {
                commandText.AppendLine(" select  top 50 *  from ExamUserDetailInfo ");
            }
            else
            {
                commandText.AppendLine(" select *  from ExamUserDetailInfo ");
            }

            sql = " where IsStop=0 and ExamStatus='HrCheck' and IsExam='true' ";
            if (!string.IsNullOrEmpty(UserCode))
            {
                sql += " and UserCode like N'%" + UserCode + "%' ";
            }
            if (!string.IsNullOrEmpty(ExamDate.ToString()))
            {
                sql += " and ExamDate < '" + ExamDate + "' ";
            }
            sql += " order by ExamDate desc";
            commandText.AppendLine(sql);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        public IDataReader Get_All_ExamInfo(DateTime ddate, DateTime examdetail)
        {
            string sql = string.Empty;
            StringBuilder commandText = new StringBuilder();
            List<DbParameter> l_parms = new List<DbParameter>();
            commandText.AppendLine(" select *  from ExamUserDetailInfo ");
            sql = " where IsStop=0 and ExamStatus='HrCheck' and IsExam='false'";           
            if (!string.IsNullOrEmpty(ddate.ToString())&& !string.IsNullOrEmpty(examdetail.ToString()))
            {
                sql += " and ExamDate>='"+ddate + "' and ExamDate < '" + examdetail + "' ";
            }
            sql += " order by ExamDate desc";
            commandText.AppendLine(sql);
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        public IDataReader GetCanSignUpAudit(string usercode)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from ( select * from ExamUserDetailInfo where TypeName=N'电子端岗位技能津贴' " +
                " and IsStop=0 and IsExam='false'   union all select * from ExamUserDetailInfo " +
                " where TypeName=N'电子端岗位技能津贴' and IsStop=0 and IsExam='true' and IsExamPass=1 )a " +
                " where a.UserCode='"+ usercode + "'"
                );
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }

        public IDataReader GetAuthority(string usercode)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select * from [dbo].[advt_users_type] where username='"+ usercode + "' and [type]='Admin'");
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        public IDataReader Get_All_ExamUserDetailInfoDianzi(string typename, string searchdata)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine(" select* from(select b.UserCode,b.UserName,b.DepartCode,b.ExamDate,b.SubjectName,c.PostID,b.State,rowid=ROW_NUMBER()over(partition by b.usercode order by examdate desc )from ExamSubject a inner join ExamUserDetailInfo b on a.SubjectName = b.SubjectName left join ExamUserInfo c on b.UserCode=c.UserCode and b.TypeName =c.TypeName where  b.IsStop=0  and b.State!=N'离职' and b.TypeName =N'电子端岗位技能津贴' and stuff(b.SubjectName, 3, 1, '')=N'测试等级' UNION ALL select b.UserCode,b.UserName,b.DepartCode,b.ExamDate,b.SubjectName,c.PostID,b.State,rowid=ROW_NUMBER()over(partition by b.usercode order by examdate desc )from ExamSubject a inner join ExamUserDetailInfo b on a.SubjectName = b.SubjectName\r\nleft join ExamUserInfo c on b.UserCode=c.UserCode and b.TypeName =c.TypeName\r\nwhere  b.IsStop=0  and b.State!=N'离职' and b.TypeName =N'电子端岗位技能津贴' and stuff(b.SubjectName, 3, 1, '')<>N'测试等级')a  where a.rowid=1  and SubjectName=N'" + typename + "'");
            //commandText.AppendLine(" select* from(select HC = isnull(a.HCLimit, ''), b.UserCode, b.UserName, b.DepartCode, b.ExamDate, b.SubjectName,c.PostID, b.State,rowid = ROW_NUMBER()over(partition by b.usercode order by UserExamDate desc)from ExamSubject a inner  join ExamUserDetailInfo b on a.SubjectName = b.SubjectName left join ExamUserInfo c on b.UserCode=c.UserCode and b.TypeName =c.TypeName  where b.IsStop = 0 and b.IsExam = 'true' and IsExamPass = 1 and b.State != N'离职' and b.TypeName = N'电子端岗位技能津贴'  )a where a.rowid = 1  and SubjectName=N'" + typename +"'");
            return DbHelper.PE.ExecuteReader(CommandType.Text, commandText.ToString());
        }
        #endregion
    }
}
