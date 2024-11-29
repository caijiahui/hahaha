using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial interface IDataProvider
    {
        #region ExamUserDetailInfo , (Ver:2.3.8) at: 2021/2/5 11:28:28

        IDataReader Get_All_ExamUserDetailInfo(object objparams);
        IDataReader Get_All_ExamUserDetailInfo(string Typename, string UserCode, string SubjectName, string DepartCode);

        IDataReader Get_All_ExamUserCheckDetail(string Typename, string UserCode, string SubjectName, string DepartCode);
        
        int Insert_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude);

        int Update_ExamUserDetailInfo(Entity.ExamUserDetailInfo info, string[] Include, string[] Exclude);

        int Delete_ExamUserDetailInfo(int ID);
        IDataReader Get_All_ExamUserInfo(DateTime date);
        IDataReader Get_All_ExamUserGetDetailInfo(string UserCode, string SubjectName, string date, string DepartCode);
        IDataReader Get_ExamUserAuditInfo(string ExamStatus, string UserCode, string typename);
        IDataReader Get_All_UserCelarInfo(string ExamStatus, string UserCode, string typename);
        IDataReader Get_All_PostCanSignUser(string UserCode);
       
        IDataReader Get_All_ExamUserALLDetailInfo(string UserCode,  string SubjectName, string TypeName, string OrgName,string DepartCode);

        IDataReader Get_UserInfo(string UserCode, DateTime? ExamDate);
        IDataReader Get_All_ExamInfo(DateTime ddate, DateTime examdetail);
        IDataReader GetCanSignUpAudit(string usercode);
        //Get_Super_UserAduitInfo
        IDataReader Get_Super_UserAduitInfo(string  ExamStatus,string username, string typename);

        IDataReader GetAuthority(string usercode);
        IDataReader Get_All_ExamUserDetailInfoDianzi(string typename, string searchdata);
        #endregion
    }
}
