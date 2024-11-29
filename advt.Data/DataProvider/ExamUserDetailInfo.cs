using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial class ExamUserDetailInfo
    {
        #region ExamUserDetailInfo , (Ver:2.3.8) at: 2021/2/5 11:28:28

        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserDetailInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserDetailInfo(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserDetailInfo(string Typename, string UserCode, string SubjectName, string DepartCode)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserDetailInfo(Typename, UserCode, SubjectName, DepartCode);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserCheckDetail(string Typename,string UserCode,string SubjectName,string DepartCode)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserCheckDetail(Typename,UserCode,SubjectName,DepartCode);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }

        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserDetailInfo()
        {
            return Get_All_ExamUserDetailInfo(null);
        }

        public static Entity.ExamUserDetailInfo Get_ExamUserDetailInfo(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserDetailInfo(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamUserDetailInfo>(reader);
        }

        public static Entity.ExamUserDetailInfo Get_ExamUserDetailInfo(int ID)
        {
            return Get_ExamUserDetailInfo(new { ID = ID });
        }

        public static int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info)
        {
            return Insert_ExamUserDetailInfo(info, null, null);
        }


        public static int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include)
        {
            return Insert_ExamUserDetailInfo(info, Include, null);
        }

        public static int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamUserDetailInfo(info, Include, Exclude);
        }

        public static int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info)
        {
            return Update_ExamUserDetailInfo(info, null, null);
        }

        public static int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include)
        {
            return Update_ExamUserDetailInfo(info, Include, null);
        }

        public static int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamUserDetailInfo(info, Include, Exclude);
        }

        public static int Delete_ExamUserDetailInfo(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamUserDetailInfo(ID);
        }
        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserInfo(DateTime date)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserInfo(date);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserGetDetailInfo(string UserCode, string SubjectName, string date, string DepartCode)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserGetDetailInfo(UserCode,SubjectName,date,DepartCode);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        //ExamStatus = "HrSignUp", IsStop = false, UserCode = item.UserCode 
        //主管审核下面根据hr报名找到所有主管管理下的人员 关键技能岗位
        public static List<Entity.ExamUserDetailInfo> Get_All_UserAduitInfo(string ExamStatus, string UserCode,string typename)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_ExamUserAuditInfo(ExamStatus, UserCode, typename);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }


        public static List<Entity.ExamUserDetailInfo> Get_All_UserCelarInfo(string ExamStatus, string UserCode, string typename)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_UserCelarInfo(ExamStatus, UserCode, typename);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        //找到主管审核下超级管理员数据
        public static List<Entity.ExamUserDetailInfo> Get_Super_UserAduitInfo(string ExamStatus, string username, string typename)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_Super_UserAduitInfo(ExamStatus,username, typename);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        //主管审核下面根据主管找到下面
        public static List<Entity.ExamUserDetailInfo> Get_All_PostCanSignUser(string UserCode)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_PostCanSignUser( UserCode);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        //serCode=data.UserCode, TypeName=data.TypeName, SubjectName=data.SubjectName , OrgName =data.OrgName, DepartCode=data.DepartCode
       
        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserALLDetailInfo(string UserCode,string SubjectName, string TypeName, string OrgName,string DepartCode)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserALLDetailInfo(UserCode, SubjectName, TypeName,OrgName, DepartCode);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        public static List<Entity.ExamUserDetailInfo> Get_UserInfo(string UserCode, DateTime? ExamDate)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_UserInfo(UserCode, ExamDate);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        
        //主管下判断是否有电子岗位报名信息
        public static Entity.ExamUserDetailInfo GetCanSignUpAudit(string usercode)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetCanSignUpAudit(usercode);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamUserDetailInfo>(reader);
        }
       
        public static List<Entity.ExamUserDetailInfo> Get_All_ExamInfo(DateTime ddate, DateTime examdetail)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamInfo(ddate, examdetail);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }
        public static List<Entity.ExamUserDetailInfo> GetAuthority(string usercode)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetAuthority(usercode);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader); 
        }
        public static List<Entity.ExamUserDetailInfo> Get_All_ExamUserDetailInfoDianzi(string typename, string searchdata)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamUserDetailInfoDianzi(typename,searchdata);
            return SqlHelper.GetReaderToList<Entity.ExamUserDetailInfo>(reader);
        }

        
        #endregion
    }
}
