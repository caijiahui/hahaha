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
        public List<Entity.ExamUserDetailInfo> ListElectronicUser { get; set; }
        public List<ElectronicUserView> ListElectronicUserView { get; set; }
        public List<advt_user_sheet> ListUsers { get; set; }

        public ExamDataModel() : base()
        {
            ListElectronicUser = new List<Entity.ExamUserDetailInfo>();
            ListElectronicUserView = new List<ElectronicUserView>();
            ListUsers = new List<advt_user_sheet>();

        }
        public void GetExamInfo(string typename)
        {
            if (typename == "电子端岗位技能津贴")
            {
                
                ListElectronicUserView = Data.ExamUserInfo.Get_All_ElectronicUserView();
                
            }
        }
        public void GetExamByTypeName(string typename,string searchdata)
        {
            //var data = Data.ExamUserInfo.Get_All_ExamUserInfo(new { SubjectName = typename, IsEnable = 0, TypeName = searchdata }).ToList();
            //var data = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { SubjectName = typename, IsStop=0, TypeName= searchdata, IsExam= "true", IsExamPass=1 }).ToList();
            var data = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfoDianzi(typename, searchdata);
            ListElectronicUser = data.Where(x => x.State != "离职").ToList();
        }
        public void GetExamUserBySubjectName(string SearchData,string subject)
        {
            var subjecs = Data.ExamSubject.Get_ExamSubject(new { SubjectName = subject });
            if (subjecs.IsProAssess)
            {
                ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ElectronicUser(SearchData,subject);
            }
            else
            {
                ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ElectronicUser(SearchData, subject);
            }
            
            
        }
        //添加完
        public bool GetSigupElectronicUser(string usercode, string UserCostCenter, string SubjectName,string sdata,string typename,string username)
        {
            List<UserInfo> Listusers = new List<UserInfo>();
            var user = Data.ExamUserInfo.Get_ExamUserInfo(new {  UserCode = usercode, TypeName=typename });
            UserInfo model = new UserInfo();
            var rule = Data.ExamRule.Get_ExamRule(new { SubjectName = SubjectName, TypeName = typename });
            model.TypeName = typename;
            model.SubjectName = SubjectName;
            model.UserCode = usercode;
            model.UserName = user.UserName;
            model.PostName = user.PostName;
            model.RuleName = rule != null ? rule.RuleName : "";
            model.DepartCode = UserCostCenter;
            model.State = user.WorkState;
            model.PostID = user.PostID;

            Listusers.Add(model);
            ExamUserInfoModel models = new ExamUserInfoModel();
            models.InsertUserDetail(Listusers, username);
            var subjecs = Data.ExamSubject.Get_ExamSubject(new { SubjectName = SubjectName });
            if (subjecs.IsProAssess)
            {
                ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ElectronicUser(sdata, SubjectName);
            }
            else
            {
                ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ElectronicUser(sdata, SubjectName);
            }
            GetExamInfo(typename);
            return true;
        }
        public bool DeleteElectronicUser(string UserCode, string SubjectNames,string typename,string username)
        {
        
            var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { UserCode = UserCode, TypeName = typename, SubjectName = SubjectNames, IsStop = 0 });
            if (c != null)
            {
                c.IsStop = true;
                c.StopCreateDate = DateTime.Now;
                c.StopCreateUser = username;
                Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(c, null, new string[] { "ID" });
            }
            var data = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfoDianzi(typename, SubjectNames);
            ListElectronicUser = data.Where(x => x.State != "离职").ToList();
            return true;
        }
        
    }





}