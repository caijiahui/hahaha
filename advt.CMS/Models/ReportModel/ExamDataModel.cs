using advt.CMS.Models.ExamModel;
using advt.Entity;
using advt.Model.ExamModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace advt.CMS.Models
{
    public class ExamDataModel
    {
        public List<Entity.U_Cancel_UserInfo> ListElectronicUser { get; set; }
        public List<ElectronicUserView> ListElectronicUserView { get; set; }
        public List<advt_user_sheet> ListUsers { get; set; }

        public ExamDataModel() : base()
        {
            ListElectronicUser = new List<Entity.U_Cancel_UserInfo>();
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
        public void GetExamByTypeName(string SubjectName, string searchdata)
        {
            var data = Data.U_Cancel_UserInfo.GetAllCancelUserInfo(SubjectName, searchdata);
            ListElectronicUser = data.Where(x => x.STATE != "离职").ToList();
        }
        public void GetExamUserBySubjectName(string SearchData,string subject)
        {
            var subjecs = Data.ExamSubject.Get_ExamSubject(new { SubjectName = subject });
            if (subjecs.IsProAssess)
            {
                ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ProElectronicUser(SearchData, subject);
            }
            else
            {
                ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ElectronicUser(SearchData, subject);
            }
            
            
        }
        //添加完
        public bool GetSigupElectronicUser(string usercode, string UserCostCenter, string SubjectName,string sdata,string typename,string username)
        {
            //插入detail
            List<UserInfo> Listusers = new List<UserInfo>();
            var user = new Entity.ExamUserInfo();

            var userss = Data.ExamUserInfo.Get_All_ExamUserInfo(new { UserCode = usercode, TypeName = typename });
            if (userss.Count()==0)
            {
                var su = Data.ExamUserInfo.Insert_ElectronicUser_usercode(usercode, UserCostCenter, SubjectName, username);
                if (su > 0)
                {  user = Data.ExamUserInfo.Get_ExamUserInfo(new { UserCode = usercode, TypeName = typename }); }
            }
            else
            {
                user = Data.ExamUserInfo.Get_ExamUserInfo(new { UserCode = usercode, TypeName = typename });

            }
           

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

            // 插入视图主表
            var cancel = new U_Cancel_UserInfo();
            cancel.UserCode = usercode;
            cancel.UserName = user.UserName;
            cancel.DepartCode = UserCostCenter;
            cancel.ExamDate = null;
            cancel.SubjectName = SubjectName;
            cancel.PostID = user.PostID;
            cancel.STATE = user.WorkState;
            cancel.InsertDate = DateTime.Now;
            Data.U_Cancel_UserInfo.Insert_U_Cancel_UserInfo(cancel, null, new string[] { "ID" });



            var subjecs = Data.ExamSubject.Get_ExamSubject(new { SubjectName = SubjectName });
            if (subjecs.IsProAssess)
            {
                ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ProElectronicUser(sdata, SubjectName);
            }
            else
            {
                ListUsers = Data.advt_user_sheet.Get_All_advt_user_sheet_ElectronicUser(sdata, SubjectName);
            }
            GetExamInfo(typename);
            return true;
        }
        public bool DeleteElectronicUser(string UserCode, string SubjectNames,string typename,string username,int ID)
        {
            //在主表删除数据取消操作
            var cancel = Data.U_Cancel_UserInfo.Get_U_Cancel_UserInfo(new { ID=ID});
            if(cancel != null)
            {
                Data.U_Cancel_UserInfo.Delete_U_Cancel_UserInfo(ID);
            }
            //插历史
            var user = new U_Cancel_UserInfo_History();
            user.UserCode = cancel.UserCode;
            user.UserName = cancel.UserName;
            user.DepartCode = cancel.DepartCode;
            user.ExamDate = cancel.ExamDate;
            user.SubjectName = cancel.SubjectName;
            user.PostID = cancel.PostID;
            user.STATE = cancel.STATE;
            user.Pre_InsertDate = cancel.InsertDate;
            user.Operate_date = DateTime.Now;
            user.Operate_User = username;

            Data.U_Cancel_UserInfo_History.Insert_U_Cancel_UserInfo_History(user, null, new string[] { "ID" });

            ListElectronicUser = Data.U_Cancel_UserInfo.GetAllCancelUserInfo(SubjectNames, typename); 
            GetExamInfo(typename);
            return true;
        }

        public bool HandleDescUser(string UserCode, string SubjectNames, string typename, string username, int ID)
        {
            bool iscancel = true;
            bool isdd = false;
            var sname = "";
            //测试A等级，先判断B下面人够不够，够在添加，不够就不可降级
            if (SubjectNames == "测试A等级")
            {
                var getadata = Data.U_Cancel_UserInfo.Get_All_U_Cancel_UserInfo(new { SubjectName = "测试B等级" });
                var subject = Data.ExamSubject.Get_All_ExamSubject(new { SubjectName = "测试B等级" });
                if (getadata.Count() < subject.FirstOrDefault().HCLimit)
                {
                    sname = "测试B等级";
                    isdd = true;
                }
            }
            if (SubjectNames == "测试B等级")
            {
                var getadata = Data.U_Cancel_UserInfo.Get_All_U_Cancel_UserInfo(new { SubjectName = "测试C等级" });
                var subject = Data.ExamSubject.Get_All_ExamSubject(new { SubjectName = "测试C等级" });
                if (getadata.Count() < subject.FirstOrDefault().HCLimit)
                {
                    sname = "测试C等级";
                    isdd = true;
                }
            }
            if (isdd == true && sname != "")
            {
                var cancel = Data.U_Cancel_UserInfo.Get_U_Cancel_UserInfo(new { ID = ID });
                if (cancel != null)
                {
                    Data.U_Cancel_UserInfo.Delete_U_Cancel_UserInfo(ID);
                }
                //插历史
                var user = new U_Cancel_UserInfo_History();
                user.UserCode = cancel.UserCode;
                user.UserName = cancel.UserName;
                user.DepartCode = cancel.DepartCode;
                user.ExamDate = cancel.ExamDate;
                user.SubjectName = cancel.SubjectName;
                user.PostID = cancel.PostID;
                user.STATE = cancel.STATE;
                user.Pre_InsertDate = cancel.InsertDate;
                user.Operate_date = DateTime.Now;
                user.Operate_User = username;
                Data.U_Cancel_UserInfo_History.Insert_U_Cancel_UserInfo_History(user, null, new string[] { "ID" });

                //插电子岗主表
                var cancels = new U_Cancel_UserInfo();
                cancels.UserCode = UserCode;
                cancels.UserName = cancel.UserName;
                cancels.DepartCode = cancel.DepartCode;
                cancels.ExamDate = DateTime.Now;
                cancels.SubjectName = sname;
                cancels.PostID = cancel.PostID;
                cancels.STATE = cancel.STATE; cancels.InsertDate = DateTime.Now;
                Data.U_Cancel_UserInfo.Insert_U_Cancel_UserInfo(cancels, null, new string[] { "ID" });

                //插入detail表
                var userinfo=Data.ExamUserInfo.Get_ExamUserInfo(new { UserCode = UserCode });
                var detail = new ExamUserDetailInfo();
                var rule = Data.ExamRule.Get_ExamRule(new { SubjectName = sname, TypeName = typename });
                detail.TypeName = typename;
                detail.SubjectName = sname;
                detail.RankName = userinfo.RankName;
                detail.EntryDate = userinfo.EntryDate;
                detail.ExamStatus = "HrCheck";
                detail.IsStop = false;
                detail.UserCode = UserCode;
                detail.UserName = user.UserName;
                detail.PostName = userinfo.PostName;
                detail.RuleName = sname;
                detail.DepartCode = user.DepartCode;
                detail.State = userinfo.WorkState;
                detail.PostID = user.PostID;
                detail.IsExam = "true";
                detail.IsExamPass = true;
                detail.ExamDate = DateTime.Now.AddMonths(-1);
                detail.Type = "降级";
                detail.OrgName = userinfo.OrgName;
                detail.DirectorCreateUser = username;
                detail.WorkPlace = userinfo.WorkPlace;
                Data.ExamUserDetailInfo.Insert_ExamUserDetailInfo(detail, null, new string[] { "ID" });
                iscancel = true;
            }
            ListElectronicUser = Data.U_Cancel_UserInfo.GetAllCancelUserInfo(SubjectNames, typename);
            GetExamInfo(typename);
            return iscancel;
        }

    }





}