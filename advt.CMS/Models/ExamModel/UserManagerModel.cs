
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using advt.BLL;
using advt.Entity;
using advt.Model.ExamModel;

namespace advt.CMS.Models.ExamModel
{
    public class UserManagerModel
    {
        public List<ExamUserMangerViewModel> ListUsers { get; set; }
        public List<string> ListType { get; set; }//权限

        public UserManagerModel() : base()
        {
            ListUsers = new List<ExamUserMangerViewModel>();

        }
        public void GetUser(SearchData VSearchData=null)
        {
            try
            {
                ListType = Data.ExamRole.Get_All_ExamRole().Select(y => y.RoleName).ToList();
                if(VSearchData!=null)
                ListUsers = Data.advt_users_type.Get_All_advt_users_join_type(VSearchData.UserNameCode,VSearchData.Depert,VSearchData.RoleName);
                else
                    ListUsers = Data.advt_users_type.Get_All_advt_users_join_type("","","");
            }
            catch (Exception ex)
            {

                throw;
            }

            
        }
        public void EditUserType(int id,string type,string username)
        {
            if (id==0)
            {
                var info = new advt_users_type();
                info.username = username;
                info.type = type;
                Data.advt_users_type.Insert_advt_users_type(info, null, new string[] { "id" });
            }
            else
            {
                var ids = Convert.ToInt32(id);
                var info = Data.advt_users_type.Get_advt_users_type(ids);
                info.type = type;
                Data.advt_users_type.Update_advt_users_type(info, null, new string[] { "id" });
            }
            GetUser();
        }
    }
    public class SearchData
    {
        public string UserNameCode { get; set; }
        public string Depert { get; set; }
        public string RoleName { get; set; }
    }
    }
