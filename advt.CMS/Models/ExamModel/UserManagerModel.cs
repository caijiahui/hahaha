
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using advt.BLL;
using advt.Entity;
namespace advt.CMS.Models.ExamModel
{
    public class UserManagerModel
    {
        public List<advt_users_type> ListUsers { get; set; }
        public List<string> ListType { get; set; }//权限
        public UserManagerModel() : base()
        {
            ListUsers = new List<advt_users_type>();
        }
        public void GetUser(string username)
        {
            try
            {
                ListType = Data.ExamRole.Get_All_ExamRole().Select(y => y.RoleName).ToList();
                if (!string.IsNullOrEmpty(username))
                {
                    ListUsers = Data.advt_users_type.Get_All_advt_users_join_type(username);
                }
                else
                {
                    ListUsers = Data.advt_users_type.Get_All_advt_users_join_type("");
                }
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
            GetUser("");
        }
    }
    }
