using advt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace advt.CMS.Models.ExamModel
{
    public class ExamRoleModel
    {
        public string RoleName { get; set; }
        public List<string> checkRole { get; set; }
        public List<ExamRole> ListExamRole { get; set; }
        public string RoleID { get; set; }
        public ExamRole VExamRole { get; set; }


        public ExamRoleModel() : base()
        {
            checkRole = new List<string>();
            ListExamRole = new List<ExamRole>();
            VExamRole = new ExamRole();
        }
        public string SaveExamRole(string username)
        {
            var Result = "";
            try
            {
                if (string.IsNullOrEmpty(RoleID))
                {
                    var Role = Data.ExamRole.Get_ExamRole(new { RoleName = RoleName });
                    if (Role != null)
                    {
                        Result = "角色已存在";
                    }
                    else
                    {
                        ExamRole rolemodel = new ExamRole();
                        rolemodel.RoleName = RoleName;
                        rolemodel.CreateUser = username;
                        rolemodel.CreateDate = DateTime.Now;
                        Data.ExamRole.Insert_ExamRole(rolemodel, null, new string[] { "ID" });
                        foreach (var item in checkRole)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                ExamRolePart rolepart = new ExamRolePart();
                                rolepart.RoleName = RoleName;
                                rolepart.PartName = item;
                                Data.ExamRolePart.Insert_ExamRolePart(rolepart, null, new string[] { "ID" });
                            }
                        }

                    }
                }
                else
                {
                    var Role = Data.ExamRole.Get_ExamRole(new { ID = RoleID });
                    Role.RoleName = RoleName;
                    Role.CreateUser = username;
                    Role.CreateDate = DateTime.Now;
                    Data.ExamRole.Update_ExamRole(Role, null, new string[] { "ID" });
                    Data.ExamRolePart.Delete_ExamRolePartByName(RoleName);
                    foreach (var item in checkRole)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            ExamRolePart rolepart = new ExamRolePart();
                            rolepart.RoleName = RoleName;
                            rolepart.PartName = item;
                            Data.ExamRolePart.Insert_ExamRolePart(rolepart, null, new string[] { "ID" });
                        }
                    }
                }

                ListExamRole = Data.ExamRole.Get_All_ExamRole();
            }
            catch (Exception ex)
            {

                throw;
            }

            return Result;
        }
        public List<ExamRole> GetExamRole(string ExamRoleName)
        {
            var model = new List<ExamRole>();
            if (!string.IsNullOrEmpty(ExamRoleName))
            {
                model = Data.ExamRole.Get_All_ExamRole(new { RoleName = ExamRoleName });
                checkRole = Data.ExamRolePart.Get_All_ExamRolePart(new { RoleName = ExamRoleName }).Select(y => y.PartName).ToList();
            }
            else
            {
                model = Data.ExamRole.Get_All_ExamRole();
            }
            return model;
        }
        public string DeleteExamRole(string RoleName)
        {
            try
            {
               
                var role = Data.ExamRole.Get_ExamRole(new { RoleName = RoleName });
                Data.ExamRole.Delete_ExamRole(role.ID);
                var rolepart = Data.ExamRolePart.Get_ExamRolePart(new { RoleName = RoleName });
                Data.ExamRolePart.Delete_ExamRolePart(rolepart.ID);
                ListExamRole = Data.ExamRole.Get_All_ExamRole();
                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
                throw;
            }
           
        }
    }
}