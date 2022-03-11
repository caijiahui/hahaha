using advt.CMS.Models.ExamModel;
using advt.Entity;
using advt.Model.ExamModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace advt.CMS.Models
{
    public class ExamDataModel
    {
        public List<ElectronicUser> ListElectronicUser { get; set; }
        public List<ElectronicUserView> ListElectronicUserView { get; set; }
        public List<advt_user_sheet> ListUsers { get; set; }

        public ExamDataModel() : base()
        {
            ListElectronicUser = new List<ElectronicUser>();
            ListElectronicUserView = new List<ElectronicUserView>();
            ListUsers = new List<advt_user_sheet>();

        }
        public void GetExamInfo(string typename)
        {
            if (typename == "电子端岗位技能津贴")
            {
                
                ListElectronicUserView = Data.ElectronicUser.Get_All_ElectronicUserView();
                
            }
        }
        public void GetExamByTypeName(string typename)
        {
            ListElectronicUser = Data.ElectronicUser.Get_All_ElectronicUser(new { SubjectName = typename, IsEnable = 0 });
        }
        public void GetExamUserBySubjectName(string SearchData)
        {
            ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ElectronicUser(SearchData);
            
        }
        public bool GetSigupElectronicUser(string usercode, string UserCostCenter, string SubjectName,string sdata,string typename,string username)
        {

            var su=Data.ElectronicUser.Insert_ElectronicUser_usercode(usercode, UserCostCenter,SubjectName, username);
            ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ElectronicUser(sdata);
            GetExamInfo(typename);
            var user = Data.ElectronicUser.Get_ElectronicUser(new { UserCode = usercode });
            UserInfo model = new UserInfo();
            List<UserInfo> Listusers = new List<UserInfo>();
            var rule = Data.ExamRule.Get_ExamRule(new { SubjectName = SubjectName, TypeName = typename });
            var exmser = Data.ExamUserInfo.Get_ExamUserInfo(new { UserCode = usercode });
            if (su > 0)
            {
                model.TypeName = typename;
                model.SubjectName = SubjectName;
                model.UserCode = usercode;
                model.UserName = user.UserName;
                model.PostName = user.ElectronicPost;
                model.RuleName = rule!=null?rule.RuleName:"";
                model.DepartCode = UserCostCenter;
                Listusers.Add(model);
                ExamUserInfoModel models = new ExamUserInfoModel();
                models.InsertUserDetail(Listusers, username);
                return true;
            }
            return false;
        }
        public bool DeleteElectronicUser(string ID,string SubjectNames,string typename,string username)
        {
            var ids = Convert.ToInt32(ID);
            var item = Data.ElectronicUser.Get_ElectronicUser(new { ID = ID });
            item.IsEnable = true;
            item.StopUser = username;
            item.StopDate = DateTime.Now;
            var su = Data.ElectronicUser.Update_ElectronicUser(item, null, new string[] { "ID" });
            ListElectronicUser = Data.ElectronicUser.Get_All_ElectronicUser(new { SubjectName = SubjectNames, IsEnable = 0 });
            GetExamInfo(typename);
            if (su > 0)
            {
                var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { UserCode = item.UserCode, TypeName = typename, SubjectName= SubjectNames, IsStop=0 });
                if (c != null)
                {
                    c.IsStop = true;
                    c.StopCreateDate = DateTime.Now;
                    c.StopCreateUser = username;
                    Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(c, null, new string[] { "ID" });
                }
               
                return true;
            }
            return false;
        }
        
    }



}