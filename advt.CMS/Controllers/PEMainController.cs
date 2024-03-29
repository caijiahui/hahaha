﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using advt.Entity;
using advt.Manager;
using System.Text.RegularExpressions;
using advt.Common;
using System.IO;
using advt.CMS;
using advt.Model.PageModel;
using advt.CMS.Models.ExamModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.Data;
using advt.CMS.Models;
using System.Text;
using FormsAuth;
using Microsoft.Ajax.Utilities;

namespace advt.Web.Controllers
{
    public class PEMainController : BaseController
    {
        [MyAuthorize]
        public ActionResult Index(string tname,string searchdata,string startdt, string enddt, int? page,int tid =1)
        {
            tname = "Engineering";
            Model.PageModel.PEModel model = new Model.PageModel.PEModel();
            model.username = this.UserNameContext.Substring(0,this.UserNameContext.Length-17);
            if (tid == 0)
            {
                return PartialView("IndexLogin",model);
            }
            ViewBag.PartialView = false;
            ViewBag.Index = page ?? 1;
            ViewBag.PageSize = 10;
            model.V_CMSCategory = Data.advt_CMSCategory.Get_advt_CMSCategory(tid);
            model.L_topics = Data.advt_topics.Get_All_Advt_topic_id(ViewBag.PageSize, ViewBag.Index, tid, searchdata,startdt,enddt);
            var count = Data.advt_topics.Get_All_Advt_topic_id(0, 0, tid, searchdata, startdt, enddt);
            ViewBag.RecordCount = count.Count;
            model.L_state = BLL.PEBLL.Get_All_State();
            model.L_categoryname = BLL.PEBLL.Get_All_CategoryName();
            model.L_Bit = BLL.PEBLL.Get_All_Bit();
            var a = Data.advt_users_type.Get_advt_users_type(this.advt_user.username);
            model.isAdministrators = false;
            model.tname = tname;
            if(a!=null)
            {
                if (a.type == "Administrators")
                {
                    model.isAdministrators = true;
                }
            }
            return View(model);
           

        }
        [MyAuthorize]
        public ActionResult Classify(string tid)
        {
            Model.PageModel.PEModel model = new Model.PageModel.PEModel();
            model.username = this.UserNameContext;
            if(!string.IsNullOrEmpty(tid))
            {
                model.tname = tid;
                return PartialView("IndexLogin", model);
            }
            return View(model);
           
        }
        [HttpPost]
        [MyAuthorize]
        public ActionResult UpdateColumn(advt_CMSCategory V_CMSCategory)
        {
            bool result = false;
            var info = BLL.PEBLL.Update_advt_CMSCategory(V_CMSCategory, null, new string[] { "id" });
            if (info == 1)
                result = true;
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [MyAuthorize]
        public ActionResult AddData(advt_topics V_topics)
        {
            bool result = false;
            Model.PageModel.PEModel model = new Model.PageModel.PEModel();
            V_topics.createdt =Convert.ToDateTime(DateTime.Now.Year+"-"+DateTime.Now.Month+"-"+DateTime.Now.Day);//上传时间
            V_topics.username = this.advt_user.username;//分享人
            V_topics.available = (byte)Status.Normal.Enable;
            var info = Data.advt_topics.Insert_advt_topics(V_topics, null, new string[] { "id" });
            if (info == 1)
                result = true;
            return Json(new { result = result, createdate = V_topics.createdt, username = V_topics.username, files = V_topics.attachment,item9 = V_topics.item9 }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult AddPPT(HttpPostedFileBase file)
        {
            bool results = false;
            string filepath = "";
            string savefile = "";
            if (file != null)
            {
                string path = Server.MapPath(_AttachmentUploadDirectory_temp);//设定上传的文件路径
                if (!Directory.Exists(path))
                {

                    Directory.CreateDirectory(path);

                }
                String gcode = System.Guid.NewGuid().ToString("N");
                string filenName = '\\' + gcode + file.FileName;

                filepath = path + filenName;

                file.SaveAs(filepath);//上传路径
                savefile ="/Attachment/temp/" + gcode + file.FileName;
                results = true;
            }
            return Json(new { result = results, filepath = savefile,filename = file.FileName }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [MyAuthorize]
        public ActionResult EditData(advt_topics V_topics)
        {
            var result = false;
            V_topics.updatedt = DateTime.Now;//更新时间
            V_topics.username = this.UserNameContext;
            var info = Data.advt_topics.Update_advt_topics(V_topics, null, new string[] { "id","createdt" });
            if (info == 1)
                result = true;
            return Json(new { result = result, update = V_topics.updatedt}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [MyAuthorize]
        public ActionResult DeleteData(advt_topics V_topics)
        {
            var result = false;
            V_topics.updatedt = DateTime.Now;//更新时间
            V_topics.available = (byte)Status.Normal.Verify;
            var info = Data.advt_topics.Update_advt_topics(V_topics, null, new string[] { "id" });
            if (info == 1)
                result = true;
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [MyAuthorize]
        public ActionResult C_Name_Info(string id)
        {
            int ids = Convert.ToInt32(id);
            var info = Data.advt_CMSCategory.Get_advt_CMSCategory(ids);
            info.categoryname.Replace("\r\n","");
            return Json(new {info = info  }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult IndexLogin()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult User_type(string userid,int? page,string tname)
        {
            ViewBag.PartialView = false;
            ViewBag.Index = page ?? 1;
            ViewBag.PageSize = 10;
            Model.PageModel.PEModel model = new Model.PageModel.PEModel();
            model.username = this.UserNameContext;
            model.L_usertype = Data.advt_users_type.Get_advt_users_type_join_user(ViewBag.PageSize, ViewBag.Index,userid);
            ViewBag.RecordCount = Data.advt_users_type.Get_advt_users_type_join_user(0, 0, userid).Count;
            model.L_Location = BLL.PEBLL.Get_All_Location();
            model.L_type = BLL.PEBLL.Get_All_L_type();
            model.tname = tname;
            return View(model);
        }

        [MyAuthorize]
        public ActionResult MaintainExamType()
        {
            ExamTypeModel model = new ExamTypeModel();
            return View(model);
        }
        [MyAuthorize]
        public ActionResult GetTypeInfo()
        {
            var model = new ExamTypeModel();
            model.GetAllVexamType();
            return Json(new { LexamType = model.LexamType, VexamType= model.VexamType }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult SaveTypeInfo(ExamTypeModel model)
        {
            var username = this.UserNameContext;
            model.SaveType(username);
            return Json(new { LexamType=model.LexamType,model.Result }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult DeleteTypeInfo(string ID)
        {
            var model = new ExamTypeModel();
            model.DeleteType(ID);
            return Json(new { model.LexamType }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize]
        public void UploadFile(int id,string filename)
        {
            var a = Data.advt_topics.Get_advt_topics(id);
            string fileName = Path.GetFileName(a.attachment);
            string filePath = Server.MapPath(a.attachment);
            Utils.ResponseFile(filePath, a.filename, ""); 
        }
        [MyAuthorize]
        public ActionResult EditUser(advt_users_type model)
        {
            var result = false;
            var isuser = Data.advt_users_type.Get_advt_users_type(model.username);
            if(isuser==null)
            {
                advt_users_type v = new advt_users_type();
                v.username = model.username;
                var info = Data.advt_users_type.Insert_advt_users_type(v, null, new string[] { "id" });
            }
            var list = Data.advt_users_type.Update_advt_users_type_username(model);
            if (list == 1)
                result = true;
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        //考试类型
        [MyAuthorize]
        public ActionResult MaintainExamSubject()
        {
            ExamSubjectModel model = new ExamSubjectModel();
            return View(model);
        }

        [MyAuthorize]
        public ActionResult GetSubjectInfo(string ExamType, string SubjectName)
        {            
            ExamSubjectModel model = new ExamSubjectModel();
            model.GetSubjectName(ExamType,SubjectName);
           return Json(new { tableData =model.ListExamSubject, VexamSubject= model.VexamSubject, LExamType=model.LExamType }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetSubjectList(string model,string subjectname)
        {
            ExamRuleModel models = new ExamRuleModel();
            var subject = Data.ExamSubject.GetSubjectList();
            models.GetRegionPlace();
            models.GetRuleType(model);
            models.GetRuleSubjectList(subjectname);
            return Json(new { ListRegionalPlace = models.ListRegionPlace, ListTypeName = subject, RuleGrList = models.RuleTopicList, ListTopic=models.ListTopic }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult SaveSubjectInfo(ExamSubjectModel model)
        {
            var username =this.UserNameContext;
            var Result = model.SaveSubject(username);
            return Json(new { Result, tableData = model.ListExamSubject }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult DeleteSubject(int model)
        {
            ExamSubjectModel mode = new ExamSubjectModel();
            mode.Delete_ExamSubject(model);
            return Json(new { tableData = mode.ListExamSubject }, JsonRequestBehavior.AllowGet);
        }

        //考试规则
        [MyAuthorize]
        public ActionResult MaintainExamRule()
        {
            ExamRuleModel model = new ExamRuleModel();
            return View(model);
        }
        [MyAuthorize]
        public ActionResult GetRuleInfo(string ExamType, string SubjectName,string RuleName)
        {
            ExamRuleModel model = new ExamRuleModel();
            model.GetRuleName(ExamType,SubjectName,RuleName);
            return Json(new { tableData = model.ListExamRule, VExamRule = model.VExamRule, LExamType=model.LExamType}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubjectName(string model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.GetSubjectList(model);
            return Json(new { ListSubjectName = models.ListExamSubject,ListTopic=models.ListTopic }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult SaveRuleInfo(ExamRuleModel model)
        {
            var username = this.UserNameContext;
            var Result = model.SaveRuleInfo(username);
            if (string.IsNullOrEmpty(Result))
            {
                model.SaveTopicInfo(model.VExamRule.RuleName);
            }
            return Json(new { Result, tableData = model.ListExamRule, RuleGrList = model.RuleTopicList }, JsonRequestBehavior.AllowGet);

        }
        [MyAuthorize]
        public ActionResult DeleteRuleInfo(int model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.Delete_ExamRule(model);
            return Json(new { tableData = models.ListExamRule }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult DeleteRuleTopicInfo(string TopicMajor, string TopicLevel, string TopicType, string RuleName,string SubjectName)
        {
            ExamRuleModel models = new ExamRuleModel();
            var Result = models.DeleteRuleTopicInfo(TopicMajor, TopicLevel, TopicType, RuleName, SubjectName);
            return Json(new { Result, RuleGrList = models.RuleTopicList }, JsonRequestBehavior.AllowGet);
        }




        [MyAuthorize]
        public ActionResult MaintainExamBank()
        {
            var model = new ExamBankModel();
            return View();
        }
        [MyAuthorize]
        public ActionResult GetBankInfo(SearchBnakData VSearchBnakData)
        {
            var model = new ExamBankModel();
            model.GetBankInfo(VSearchBnakData);
            return Json(new { LExamBank = model.LExamBank, LExamType =model.LExamType, BankRemark=model.BankRemark }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetTopic(int ID)
        {
            var model = new ExamBankModel();
            model.GetTopic(ID);
            return Json(new { VExamBank = model.VExamBank}, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult DeleteBankInfo(SearchBnakData VSearchBnakData)
        {
            var model = new ExamBankModel();
            var count = model.DeleteBankInfo(VSearchBnakData);
            var result = count+" ";

            return Json(new { Result = result, LExamBank= model.LExamBank, BankRemark=model.BankRemark });


        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult SaveBankInfo(ExamBankModel model)
        {
            var username = this.UserNameContext;
            model.SaveBankInfo(username);
            return Json(new { model.LExamBank}, JsonRequestBehavior.AllowGet);
        }
        public FileResult SignUpBankExcel(string ExamType, string ExamSubject, string ExamMajor, string ExamLevel, string ExamContent)
        {
            var model = new ExamBankModel();
            if (ExamType == "undefined")
                ExamType = "";
            if (ExamSubject == "undefined")
                ExamSubject = "";
            if (ExamMajor == "undefined")
                ExamMajor = "";
            if (ExamLevel == "undefined")
                ExamLevel = "";
            if (ExamContent == "undefined")
                ExamContent = "";
            var ms= model.SignUpBank(ExamType, ExamSubject, ExamMajor, ExamLevel, ExamContent);
            return File(ms, "application/vnd.ms-excel", ExamType + ExamSubject + "题库" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

        }


        //Excel上传题库
        public JsonResult Upload_TEL_MASTER(HttpPostedFileBase file)
        {
            string filepath = "";
            var username = this.UserNameContext;
            var LBank = new List<ExamBank>();
            if (file != null)
            {
                string path = Server.MapPath(_AttachmentUploadDirectory_temp);//设定上传的文件路径
                if (!Directory.Exists(path))
                {

                    Directory.CreateDirectory(path);

                }
                String gcode = System.Guid.NewGuid().ToString("N");
                string filenName = '\\' + gcode + file.FileName;

                filepath = path + filenName;

                file.SaveAs(filepath);//上传路径
            }
            var model = new ExamBankModel();
            model.UploadBank(filepath, username);

            return Json(new {Result=model.Result, model.LExamBank }, JsonRequestBehavior.AllowGet);
        }
        //Excel上传实践
        public JsonResult Upload_TEL_Practice(HttpPostedFileBase file)
        {
            string filepath = "";
            var username = this.UserNameContext;
            if (file != null)
            {
                string path = Server.MapPath(_AttachmentUploadDirectory_temp);//设定上传的文件路径
                if (!Directory.Exists(path))
                {

                    Directory.CreateDirectory(path);

                }
                String gcode = System.Guid.NewGuid().ToString("N");
                string filenName = '\\' + gcode + file.FileName;

                filepath = path + filenName;

                file.SaveAs(filepath);//上传路径
            }
            var model = new SupervisorAuditModel();
            model.UploadPractice(filepath,username);

            return Json(new { Result = model.Result}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Upload_BankPic(HttpPostedFileBase file)
        {
            if (file != null)
            {
                //判断传入图片大小是否小于1MB
                if (file.ContentLength <= 1048576)
                {
                    //判断是否是图片类型
                    if (file.FileName.IndexOf(".jpg") > 0 || file.FileName.IndexOf(".png") > 0 || file.FileName.IndexOf(".jpeg") > 0 || file.FileName.IndexOf(".gif") > 0)
                    {
                        DataTable dt = new DataTable();
                        string filepath = "";
                        string path = Server.MapPath(_AttachmentBankPic);//设定上传的文件路径
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string filenName = '\\' + file.FileName;
                        filepath = path + filenName;
                        if (Directory.Exists(filepath))
                        {

                            return Json(new { Result = "已有相同名称图片" }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {


                            file.SaveAs(filepath);//上传路径
                        }
                    }
                    else
                    {
                        return Json(new { Result = "只可传入格式.jpg/.png/.jpeg/.gif文件" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { Result = "图片大小大于1MB" }, JsonRequestBehavior.AllowGet);
                }
            }

           
            return Json(new { Result="成功" }, JsonRequestBehavior.AllowGet);
        }

        


        [MyAuthorize]
        [HttpPost]
        public ActionResult FinishTopic(ExamPageModel model)
        {
            model.GetExam();
            return Json(new { examList = model.examList, ListBank = model.ListBankView, nowItem = model.nowItem, total = model.total }, JsonRequestBehavior.AllowGet);
        }
        //考试个人基本信息
        [MyAuthorize]
        public ActionResult MaintainExamUserInfo()
        {
            ExamUserInfoModel model = new ExamUserInfoModel();            
            return View(model);
        }
        [MyAuthorize]
        public ActionResult GetUserInfo(SearchUserData data)
        {
            ExamUserInfoModel model = new ExamUserInfoModel();
            model.GetUserInfo(data);
            return Json(new { tableData = model.ListUserInfo11, YListUserInfo = model.YListUserInfo, CPListUserInfo = model.ListDetailInfo, LExamType=model.LExamType, LWorkPlace = model.LWorkPlace }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Upload_UserAch(HttpPostedFileBase file)
        {
            string name= this.UserNameContext;
            string filepath = "";
            if (file != null)
            {
                string path = Server.MapPath(_AttachmentUploadDirectory_temp);//设定上传的文件路径
                if (!Directory.Exists(path))
                {

                    Directory.CreateDirectory(path);

                }
                String gcode = System.Guid.NewGuid().ToString("N");
                string filenName = '\\' + gcode + file.FileName;

                filepath = path + filenName;

                file.SaveAs(filepath);//上传路径
            }
            var model = new ExamUserInfoModel();
            model.UploadUser(filepath, name);
            SearchUserData data = new SearchUserData();
            model.GetUserInfo(data);
            return Json(new { Result = model.Result, tableData = model.ListUserInfo }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize]
        public ActionResult GetExamUser(int ID)
        {
            var model = new ExamUserInfoModel();
            model.GetExamUser(ID);
            return Json(new { VExamUserInfo = model.VExamUserInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult InsertUserDetailInfo(List<UserInfo> model, SearchUserData data)
        {
            var username = this.UserNameContext;
            ExamUserInfoModel models = new ExamUserInfoModel();
            var result=models.InsertUserDetail(model, username);
            models.GetUserInfo(data);
            models.GetUserComInfo();
            return Json(new { tableData = models.ListUserInfo, YListUserInfo = models.YListUserInfo, CPListUserInfo = models.ListDetailInfo, Results = result });
        }

        [MyAuthorize]
        public ActionResult SyncQuantityPoint(SearchUserData data)
        {
            var username = this.UserNameContext;
            ExamUserInfoModel models = new ExamUserInfoModel();
            var startdate = string.Format("{0:yyyy/MM/01}", DateTime.Now.AddMonths(-6));
            var sd = string.Format("{0:yyyy/MM/01}", DateTime.Now.AddMonths(0));
            var enddate = string.Format("{0:yyyy/MM/28}", DateTime.Now.AddMonths(0));
            models.GetUserInfo(data);
            var userinfo = models.ListUserInfo11.Where(x => !string.IsNullOrEmpty(x.ReadExamDate) && Convert.ToDateTime(x.ReadExamDate).CompareTo(Convert.ToDateTime(sd)) >= 0 && Convert.ToDateTime(x.ReadExamDate).CompareTo(Convert.ToDateTime(enddate)) < 0).Select(x => x.UserCode);
            var userlist = string.Join(",", userinfo);

            var result = models.GetChassisAchieveUser(startdate, enddate, username, sd, userlist);
            //models.GetUserInfo(data);
            models.GetUserComInfo();
            return Json(new { tableData = models.ListUserInfo, YListUserInfo = models.YListUserInfo, CPListUserInfo = models.ListDetailInfo, Results = result });
        }


        [MyAuthorize]
        public ActionResult DeleteExamUserInfo(int model, SearchUserData data)
        {
            ExamUserInfoModel models = new ExamUserInfoModel();
            models.DeleteExamUserInfo(model);
            models.GetUserInfo(data);
            return Json(new { tableData = models.ListUserInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult ReverseExamUser(int model)
        {
            ExamUserInfoModel models = new ExamUserInfoModel();
            var username = this.UserNameContext;
            models.ReverseExamUserInfo(model,username);
            SearchUserData data = new SearchUserData();
            models.GetUserInfo(data);
            return Json(new { tableData = models.ListUserInfo }, JsonRequestBehavior.AllowGet);
        }


        [MyAuthorize]
        public ActionResult SaveVExamUserInfo(ExamUserInfoModel model, SearchUserData data)
        {
            var username = this.UserNameContext;
            model.SaveUserInfo(username);
            model.GetUserInfo(data);
            return Json(new { tableData = model.ListUserInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult StopComplete(string model)
        {
            ExamUserInfoModel models = new ExamUserInfoModel();
            var username = this.UserNameContext;
            models.StopComplete(model, username);

            models.GetUserComInfo();
            return Json(new { CPListUserInfo = models.ListDetailInfo });
        }


        //主管审核
        [MyAuthorize]
        public ActionResult SupervisorAudit()
        {
            SupervisorAuditModel model = new SupervisorAuditModel();
            return View(model);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult GetSupervisorAuditUser()
        {
            SupervisorAuditModel model = new SupervisorAuditModel();
            var name = this.UserNameContext;
            model.GetAllExamUserDetailInfo(name);
            return Json(new { LCheckAudtiUser = model.LCheckAudtiUser, LRules= model.LRules, LSignedupUser=model.LSignedupUser, LExamType=model.LExamType,model.LSuperExamType });
        }
        public JsonResult Upload_Supervisor(HttpPostedFileBase file)
        {
            string filepath = "";
            string filenName = "";
            if (file != null)
            {
                string path = Server.MapPath(_AttachmentPractice);//设定上传的文件路径
                if (!Directory.Exists(path))
                {

                    Directory.CreateDirectory(path);

                }
                String gcode = System.Guid.NewGuid().ToString("N");
                filenName = '\\' + gcode + file.FileName;

                filepath = path + filenName;

                file.SaveAs(filepath);//上传路径
            }
            return Json(new { fileName = filenName }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult SearchPracticeByCode(string model)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            models.SearchPracticeInfo(model);
            return Json(new { LPracticeInfo = models.LPracticeInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult SavePracticeInfo(PracticeInfo model,int DetailId)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            var username = this.UserNameContext;
            models.InsertPracticeInfo(model,username, DetailId);
            return Json(new {}, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult InsertUserDetail(List<ExamUserDetailInfo> model)
        {
            var username = this.UserNameContext;
            SupervisorAuditModel models = new SupervisorAuditModel();
            var Result=models.InsertUserDetail(model,username);

            return Json(new { LCheckAudtiUser = models.LCheckAudtiUser, LSignedupUser= models.LSignedupUser, Result });
        }
       
        [MyAuthorize]
        [HttpPost]
        public ActionResult SerachDetail(string model)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            models.SerachDetailByUserCode(model);

            return Json(new { LExamUserDetailInfo=models.LExamUserDetailInfo });
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult StopUser(string model)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            var username = this.UserNameContext;
            models.Stopuser(model, username);
            return Json(new { LCheckAudtiUser = models.LCheckAudtiUser,  LSignedupUser = models.LSignedupUser });
        }
        //超级管理员搜索

        [MyAuthorize]
        [HttpPost]
        public ActionResult SearchSuperUser(string typename, string subject, string sData)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            var username = this.UserNameContext;
            var LSuperUsers = models.GetSuperUserByUsercode(typename, subject, sData, username);
            return Json(new { LSuperUsers });
        }
        //超级管理员增加人员

        [MyAuthorize]
        [HttpPost]
        public ActionResult InsertSuper(string usercode, string SubjectName, string typename, string depart,string WorkPlace,string sData)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            var username = this.UserNameContext;
            var LSuperUsers = models.InsertSuper(usercode, SubjectName, typename, username, depart, WorkPlace, sData);
            return Json(new { LSuperUsers });
        }



        //Hr审核
        [MyAuthorize]
        public ActionResult HrAudit()
        {
            HrAuditModel model = new HrAuditModel();
            return View(model);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult GetHrAuditUser(SearchHrData model)
        {
            HrAuditModel models = new HrAuditModel();
            models.GetHrAuditUser(model);
            return Json(new { ListHrAuditUser = models.ListHrAuditUser, ListHrAuditSuccessUser = models.ListHrAuditSuccessUser,LExamType= models.LExamType,models.ListOrgName,models.ListWorkPlace });
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult UpdateHrAduitUser(List<ExamUserDetailInfo> model)
        {
            var username = this.UserNameContext;
            HrAuditModel models = new HrAuditModel();
             models.UpdateHrAduitUser(model, username);
            return Json(new { ListHrAuditUser = models.ListHrAuditUser, ListHrAuditSuccessUser = models.ListHrAuditSuccessUser });
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult StopHrAuditUser(string model)
        {
            HrAuditModel models = new HrAuditModel();
            var username = this.UserNameContext;
            models.StopHrAuditUser(model, username);
            return Json(new { ListHrAuditUser = models.ListHrAuditUser, ListHrAuditSuccessUser = models.ListHrAuditSuccessUser,models.ListProcessUser });
        }
        //上传考试资格
        public JsonResult Upload_Qualification(HttpPostedFileBase file)
        {
            string filepath = "";

            var username = this.UserNameContext;
            if (file != null)
            {
                string path = Server.MapPath(_AttachmentUploadDirectory_temp);//设定上传的文件路径
                if (!Directory.Exists(path))
                {

                    Directory.CreateDirectory(path);

                }
                String gcode = System.Guid.NewGuid().ToString("N");
                string filenName = '\\' + gcode + file.FileName;

                filepath = path + filenName;

                file.SaveAs(filepath);//上传路径
            }
            var model = new HrAuditModel();
            model.Qualification(filepath, username);

            return Json(new { Result = model.Result, ListHrAuditUser = model.ListHrAuditUser, ListHrAuditSuccessUser = model.ListHrAuditSuccessUser }, JsonRequestBehavior.AllowGet);
        }

        //上传实践成绩
        public JsonResult Upload_Practice(HttpPostedFileBase file)
        {
            string filepath = "";

            var username = this.UserNameContext;
            if (file != null)
            {
                string path = Server.MapPath(_AttachmentUploadDirectory_temp);//设定上传的文件路径
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                String gcode = System.Guid.NewGuid().ToString("N");
                string filenName = '\\' + gcode + file.FileName;

                filepath = path + filenName;

                file.SaveAs(filepath);//上传路径
            }
            var model = new HrAuditModel();
            model.UploadPractice(filepath, username);
            return Json(new { Result = model.Result, ListHrAuditUser = model.ListHrAuditUser, ListHrAuditSuccessUser = model.ListHrAuditSuccessUser }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize]
        [HttpPost]
        public ActionResult SearchPracticeUserDetail(string model)
        {
            HrAuditModel models = new HrAuditModel();
            var username = this.UserNameContext;
            models.SearchPracticeUserDetail(model);
            return Json(new { LExamUserDetailInfo = models.LExamUserDetailInfo, LPracticeInfo = models.LPracticeInfo });
        }

        [MyAuthorize]
        [HttpPost]
        public ActionResult SearchPracticeInfo(string model)
        {
            ExamUserInfoModel models = new ExamUserInfoModel();
            models.SearchPracticeInfo(model);
            return Json(new { LPracticeInfo = models.LPracticeInfo }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize]
        [HttpPost]
        public ActionResult SerachDetailInfo(string model)
        {
            ExamUserInfoModel models = new ExamUserInfoModel();
            models.SerachDetailByUserCode(model);

            return Json(new { LExamUserDetailInfo = models.LExamUserDetailInfo });
        }


        [MyAuthorize]
        [HttpPost]
        public ActionResult SerachMeritsRecord(string model)
        {
            ExamUserInfoModel models = new ExamUserInfoModel();
            models.SerachMeritsRecord(model);
            return Json(new { LExamUserMeritsRecord = models.LExamUserMeritsRecord });
        }

        public ActionResult GetRuleTypeName(string model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.GetSubjectList(model);
            return Json(new { ListSubjectName = models.ListExamSubject, ListTopic = models.ListTopic, LWorkPlace=models.LWorkPlace }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrgRuleName(string model)
        {
            RegionalPostModel models = new RegionalPostModel();
            models.GetOrgType(model);
            return Json(new { LExamTypess = models.LExamTypess, ListDepartCode=models.ListDepartCode }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDepartRuleName(string model)
        {
            RegionalPostModel models = new RegionalPostModel();
            models.GetDepartRuleName(model);
            return Json(new { LExamTypess = models.LExamTypess}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRuleSubjectName(string model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.GetRuleSubjectList(model);
            return Json(new { ListTopic = models.ListTopic }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRegionalDeapart(string model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.GetRegionalDeapart(model);
            return Json(new { ListDepartCode = models.ListRegionDepart }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPostName(string model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.GetPostName(model);
            return Json(new { ListPostName = models.ListPostName }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult Upload_UserInfo(HttpPostedFileBase file)
        {
            string filepath = "";
            if (file != null)
            {
                string path = Server.MapPath(_AttachmentUploadDirectory_temp);//设定上传的文件路径
                if (!Directory.Exists(path))
                {

                    Directory.CreateDirectory(path);

                }
                String gcode = System.Guid.NewGuid().ToString("N");
                string filenName = '\\' + gcode + file.FileName;

                filepath = path + filenName;

                file.SaveAs(filepath);//上传路径
            }
            var model = new ExamUserInfoModel();
            model.UploadUserInfo(filepath);   
            
            return Json(new { Result = model.Result, CPListUserInfo = model.ListDetailInfo }, JsonRequestBehavior.AllowGet);
        }
        public FileResult ShunFengExcel(string TypeName,string UserCode,string DepartCode,string ReadyExamDate,string WorkPlace)
        {
            try
            {
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                ExamUserInfoModel models = new ExamUserInfoModel();
                SearchUserData data = new SearchUserData();
                data.TypeName = TypeName;
                if (UserCode == "undefined")
                {
                    UserCode =null;
                }
                data.UserCode = UserCode;
                if (DepartCode == "undefined")
                {
                    DepartCode = null;
                }
                data.DepartCode = DepartCode;
                if (ReadyExamDate == "undefined")
                {
                    ReadyExamDate = null;
                }
                data.ReadyExamDate = ReadyExamDate;
                if (string.IsNullOrEmpty(WorkPlace))
                {
                    WorkPlace = null;
                }
                data.WorkPlace = WorkPlace;
                models.GetUserInfo(data);
                //获取list数据
                var tlst = models.ListUserInfo11;
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("区域");
                row1.CreateCell(1).SetCellValue("部门");
                row1.CreateCell(2).SetCellValue("工号");
                row1.CreateCell(3).SetCellValue("姓名");
                row1.CreateCell(4).SetCellValue("入职日期");
                row1.CreateCell(5).SetCellValue("职等");
                row1.CreateCell(6).SetCellValue("职位");
                row1.CreateCell(7).SetCellValue("入职初始等级");
                row1.CreateCell(8).SetCellValue("考核通过等级");
                row1.CreateCell(9).SetCellValue("考核通过等级");
                row1.CreateCell(10).SetCellValue("最高可考等级");
                row1.CreateCell(11).SetCellValue("下次可考等级");
                row1.CreateCell(12).SetCellValue("每年应复审时间段");
                row1.CreateCell(13).SetCellValue("科目");
                row1.CreateCell(14).SetCellValue("规则");
                row1.CreateCell(15).SetCellValue("下次待考时间");
                //将数据逐步写入sheet1各个行
                for (int i = 0; i < tlst.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(tlst[i].WorkPlace);//工号
                    rowtemp.CreateCell(1).SetCellValue(tlst[i].DepartCode);//工号
                    rowtemp.CreateCell(2).SetCellValue(tlst[i].UserCode);//工号
                    rowtemp.CreateCell(3).SetCellValue(tlst[i].UserName);//姓名
                    rowtemp.CreateCell(4).SetCellValue(tlst[i].EntryDate.ToString());//入职日期
                    rowtemp.CreateCell(5).SetCellValue(tlst[i].RankName);//职等
                    rowtemp.CreateCell(6).SetCellValue(tlst[i].PostName);//职位
                    rowtemp.CreateCell(7).SetCellValue(tlst[i].ReverseBuckle);
                    rowtemp.CreateCell(8).SetCellValue(tlst[i].CurrentSkillLevel);
                    if (!string.IsNullOrEmpty(tlst[i].LastExamTime.ToString()))
                    {
                        var last = tlst[i].LastExamTime.ToString();
                        var lasttime = string.Format("{0}-{1}", Convert.ToDateTime(last).Year, Convert.ToDateTime(last).Month);
                        rowtemp.CreateCell(9).SetCellValue(lasttime);
                    }
                    //rowtemp.CreateCell(9).SetCellValue(tlst[i].LastExamTime.ToString());
                    rowtemp.CreateCell(10).SetCellValue(tlst[i].HighestTestSkill);
                    rowtemp.CreateCell(11).SetCellValue(tlst[i].ApplicationLevel);
                    rowtemp.CreateCell(12).SetCellValue(tlst[i].ExamineMonth);
                    rowtemp.CreateCell(13).SetCellValue(tlst[i].SubjectName);
                    rowtemp.CreateCell(14).SetCellValue(tlst[i].RuleName);
                    //rowtemp.CreateCell(15).SetCellValue(tlst[i].ReadExamDate);
                    if (!string.IsNullOrEmpty(tlst[i].ReadExamDate))
                    {
                        var ss = tlst[i].ReadExamDate.ToString();
                        var examendtime = string.Format("{0}-{1}", Convert.ToDateTime(ss).Year, Convert.ToDateTime(ss).Month);
                        rowtemp.CreateCell(15).SetCellValue(examendtime);
                    }
                    else {
                        rowtemp.CreateCell(15).SetCellValue(tlst[i].ReadExamDate);
                    }

                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "人员信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception)
            {
                throw;
            }

        }
        public FileResult ExamExcel(string TypeName, string UserCode, string DepartCode, string ReadyExamDate, string WorkPlace)
        {
            try
            {
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                ExamUserInfoModel models = new ExamUserInfoModel();
                SearchUserData data = new SearchUserData();
                data.TypeName = TypeName;
                if (UserCode == "undefined")
                {
                    UserCode = null;
                }
                data.UserCode = UserCode;
                if (DepartCode == "undefined")
                {
                    DepartCode = null;
                }
                data.DepartCode = DepartCode;
                if (ReadyExamDate == "undefined")
                {
                    ReadyExamDate = null;
                }
                data.ReadyExamDate = ReadyExamDate;
                if (string.IsNullOrEmpty(WorkPlace))
                {
                    WorkPlace = null;
                }
                data.WorkPlace = WorkPlace;
                models.GetUserInfo(data);
                //获取list数据
                var tlst = models.YListUserInfo;
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("考试类型");
                row1.CreateCell(1).SetCellValue("工号");
                row1.CreateCell(2).SetCellValue("姓名");
                row1.CreateCell(3).SetCellValue("部门");
                row1.CreateCell(4).SetCellValue("本次申请等级");
                row1.CreateCell(5).SetCellValue("科目");
                row1.CreateCell(6).SetCellValue("规则");
                row1.CreateCell(7).SetCellValue("区域");
               
                //将数据逐步写入sheet1各个行
                for (int i = 0; i < tlst.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(tlst[i].TypeName);//工号
                    rowtemp.CreateCell(1).SetCellValue(tlst[i].UserCode);//工号
                    rowtemp.CreateCell(2).SetCellValue(tlst[i].UserName);//姓名
                    rowtemp.CreateCell(3).SetCellValue(tlst[i].DepartCode);//工号
                    rowtemp.CreateCell(4).SetCellValue(tlst[i].ApplicationLevel);
                    rowtemp.CreateCell(5).SetCellValue(tlst[i].SubjectName);
                    rowtemp.CreateCell(6).SetCellValue(tlst[i].RuleName);

                    rowtemp.CreateCell(7).SetCellValue(tlst[i].WorkPlace);//工号
                    

                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "考试人员信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception)
            {
                throw;
            }
        }
        public FileResult SignUpExamExcel()
        {
    /* string TypeName,string Usercode,string DepartCode*/
            try
            {
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                ExamUserInfoModel models = new ExamUserInfoModel();
                
                models.GetUserComInfo();
                //获取list数据
                var tlst = models.ListDetailInfo;
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("工号");
                row1.CreateCell(1).SetCellValue("状态");
                row1.CreateCell(2).SetCellValue("考试类型");
                row1.CreateCell(3).SetCellValue("姓名");
                row1.CreateCell(4).SetCellValue("部门代码");
                row1.CreateCell(5).SetCellValue("本职等对应技能等级");
                row1.CreateCell(6).SetCellValue("最高可考技能等级");
                row1.CreateCell(7).SetCellValue("本次申请技能等级");
                //将数据逐步写入sheet1各个行
                for (int i = 0; i < tlst.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(tlst[i].UserCode);//工号
                    rowtemp.CreateCell(2).SetCellValue(tlst[i].ExamStatus);//状态
                    rowtemp.CreateCell(3).SetCellValue(tlst[i].TypeName);//考试类型
                    rowtemp.CreateCell(4).SetCellValue(tlst[i].UserName);//姓名
                    rowtemp.CreateCell(5).SetCellValue(tlst[i].DepartCode);//部门代码
                    rowtemp.CreateCell(6).SetCellValue(tlst[i].SkillName);//本职等对应技能等级
                    rowtemp.CreateCell(7).SetCellValue(tlst[i].HighestLevel);//最高可考技能等级
                    rowtemp.CreateCell(8).SetCellValue(tlst[i].ApplyLevel);//本次申请技能等级
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "HR确认考试人员名单" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception)
            {
                throw;
            }

        }


       





        //总页面报表
        [MyAuthorize]
        public ActionResult MaintainExamPage()
        {
            MaintainExamPageModel model = new MaintainExamPageModel();
            return View(model);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult GetMaintainExamPage(SerarchData data)
        {
            MaintainExamPageModel model = new MaintainExamPageModel();
            model.GetPageInfo(data);
            return Json(new { tableData=model.ListPageInfo, LExamType=model.LExamType, LWorkPlace=model.LWorkPlace });
        }
        //权限管控
        [MyAuthorize]
        public ActionResult UserManager()
        {
            UserManagerModel model = new UserManagerModel();
            return View(model);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult GetUserManagerInfo(SearchData VSearchData)
        {
            UserManagerModel model = new UserManagerModel();
            model.GetUser(VSearchData);
            return Json(new { ListUsers = model.ListUsers, ListType=model.ListType });
        }
        [MyAuthorize]
        public ActionResult EditUserType(int id,string type, string username)
        {
            UserManagerModel model = new UserManagerModel();
            model.EditUserType(id, type, username);
            return Json(new { ListUsers = model.ListUsers, ListType = model.ListType });

        }
        [MyAuthorize]
        public ActionResult MaintainExamRole()
        {
            var model = new ExamRoleModel();
            return View(model);
        }
        [MyAuthorize]
        public ActionResult SaveRoleInfo(ExamRoleModel model)
        {
            var username = this.UserNameContext;
            var Result = model.SaveExamRole(username);
            return Json(new { Result, ListExamRole=model.ListExamRole }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetExamRole(string ExamRoleName)
        {
            var model = new ExamRoleModel();
            var roles=model.GetExamRole(ExamRoleName);
            return Json(new { tableData = roles, checkRole = model.checkRole, LRole=model.LRole }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult DeleteExamRole(string ExamRoleName)
        {
            var model = new ExamRoleModel();
            var result = model.DeleteExamRole(ExamRoleName);
            return Json(new { result , tableData=model.ListExamRole }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetBankPic()
        {
            var model = new ExamRoleModel();
            List<string> ja = new List<string>();
            var path1 = System.AppDomain.CurrentDomain.BaseDirectory;//获取程序集目录
            string path = Path.Combine(path1, "Attachment", "BankPic");//Path.Combine 将3个字符串组合成路径
            var images = Directory.GetFiles(path, ".", SearchOption.AllDirectories).Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".gif"));
            //images = Directory.GetFiles(path, "*.png|*.jpg", SearchOption.AllDirectories);
            //Directory.GetFiles 返回指定目录的文件路径 SearchOption.AllDirectories 指定搜索当前目录及子目录

            //遍历string 型 images数组
            foreach (var i in images)
            {
                var str = i.Replace(path1, "");//获取相对路径
                var names = i.Replace(path+"\\", "");
                var path2 = str.Replace("\\", "/");
                model.LPic.Add(new PicData { PicName=names,PicPath="/"+path2 });
            }

            return Json(new { LPic=model.LPic}, JsonRequestBehavior.AllowGet);
        }
        public FileResult ExportPageExcel(string UserCode, string SubjectName, string TypeName, string OrgName, string DepartCode)
        {
            var model = new MaintainExamPageModel();


            SerarchData data = new SerarchData();
            data.UserCode = UserCode == "undefined" ? null : UserCode;
            data.SubjectName = SubjectName == "undefined" ? null : SubjectName;
            data.OrgName = OrgName == "undefined" ? null : OrgName;
            data.TypeName = TypeName == "undefined" ? null : TypeName;
            data.DepartCode = DepartCode == "undefined" ? null : DepartCode;
            model.GetPageInfo(data);
            //获取list数据
            var tlst = model.ListPageInfo;
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("部门代码");
            row1.CreateCell(1).SetCellValue("工号");
            row1.CreateCell(2).SetCellValue("姓名");
            row1.CreateCell(3).SetCellValue("入职日期");
            row1.CreateCell(4).SetCellValue("职级");
            
            row1.CreateCell(5).SetCellValue("考试类型");
            row1.CreateCell(6).SetCellValue("考试科目");
            row1.CreateCell(7).SetCellValue("考试日期");
            row1.CreateCell(8).SetCellValue("考试结果");
            row1.CreateCell(9).SetCellValue("岗位等级");
            row1.CreateCell(10).SetCellValue("岗位津贴");
            row1.CreateCell(11).SetCellValue("技能津贴");
            row1.CreateCell(12).SetCellValue("专业加给");
            row1.CreateCell(13).SetCellValue("津贴总额");
            row1.CreateCell(14).SetCellValue("本次考核后增加总额");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < tlst.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(tlst[i].DepartCode);
                rowtemp.CreateCell(1).SetCellValue(tlst[i].UserCode);
                rowtemp.CreateCell(2).SetCellValue(tlst[i].UserName);
                rowtemp.CreateCell(3).SetCellValue(tlst[i].EntryDate.ToString());
                rowtemp.CreateCell(4).SetCellValue(tlst[i].RankName);
               
                rowtemp.CreateCell(5).SetCellValue(tlst[i].TypeNameOne);
                rowtemp.CreateCell(6).SetCellValue(tlst[i].SubjectNameOne);
                rowtemp.CreateCell(7).SetCellValue(Convert.ToDateTime(tlst[i].ExamDateOne));
                rowtemp.CreateCell(8).SetCellValue(tlst[i].ExamResultOne);
                rowtemp.CreateCell(9).SetCellValue(tlst[i].PostQuotaOne);
                rowtemp.CreateCell(10).SetCellValue(tlst[i].ElectronicQuotaOne);
                rowtemp.CreateCell(11).SetCellValue(tlst[i].SkillsAllowanceOne);
                rowtemp.CreateCell(12).SetCellValue(tlst[i].MajorQuotaOne);
                rowtemp.CreateCell(13).SetCellValue(tlst[i].TotalQuotaOne);
                rowtemp.CreateCell(14).SetCellValue(tlst[i].AddData);

            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        //岗位
        [MyAuthorize]
        public ActionResult MaintainRegionalPost()
        {
            RegionalPostModel model = new RegionalPostModel();
            return View(model);
        }

        [MyAuthorize]
        public ActionResult GetRegionalPost(string RuleName, string PostName,string RuleTwoName)
        {
            RegionalPostModel model = new RegionalPostModel();           
            model.GetPostName(RuleName, PostName, RuleTwoName,null,null);
            return Json(new { tableData = model.ListRegionalPost, VregionalPost = model.VregionalPost, LExamRule = model.LExamRule, LExamRuleTwo=model.LExamRuleTwo, LPostType=model.LPostType, LPostCycleType=model.LPostCycleType}, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult SavePostInfo(RegionalPostModel model)
        {
            var username = this.UserNameContext;
            var Result = model.SavePostInfo(username);
            return Json(new { Result, tableData = model.ListRegionalPost }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetRegionalPostList(string postid,string departcode,string postname)
        {
            var LExamRules = Data.ExamRule.Get_All_ExamRule();
            var LExamType = Data.ExamType.Get_All_ExamType();
            RegionalPostModel model = new RegionalPostModel();
            model.GetPostName(null, postname, null,postid,departcode);
            return Json(new { ListExamRule = LExamRules, ListExamRuleTwo= LExamRules, ListExamType=LExamType, LPostType = model.LPostType, LPostCycleType = model.LPostCycleType, ListArea = model.ListArea, LPostRank = model.LPostRank, LPostExamEntry =model.LPostExamEntry , ListExamRuleThree = LExamRules, PostRuleList=model.ListExamPostRule }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetRuleList(string id,string postname,string departcode)
        {
            var LExamRules = Data.ExamRule.Get_All_ExamRule();
            RegionalPostModel models = new RegionalPostModel();
            models.GetRulePost(id,postname,departcode);
            return Json(new {  ListExamRuleTwo = LExamRules, LPostExamEntry = models.LPostExamEntry, PostRuleList = models.ListExamPostRule}, JsonRequestBehavior.AllowGet);
        }
        
        [MyAuthorize]
        public ActionResult GetPostDepart(string model)
        {
            RegionalPostModel models = new RegionalPostModel();
            models.GetPostDepart(model);
            return Json(new { ListPostDepart = models.ListPostDepart }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetExamTypeInfo(string model)
        {
            RegionalPostModel models = new RegionalPostModel();
            models.GetExamTypeInfo(model);
            return Json(new { ListExamRuleTwo = models.ListExamRuleTwo,LPostExamEntry=models.LPostExamEntry }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize]
        public ActionResult GetDepartPostName(string model)
        {
            RegionalPostModel models = new RegionalPostModel();
            models.GetPostName(model);
            return Json(new { ListPostName = models.ListPostName }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize]
        public ActionResult GetDepartPostID(string model)
        {
            RegionalPostModel models = new RegionalPostModel();
            var postid = string.Empty;
            postid =models.GetPostID(model);
            return Json(new { PostID = postid }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize]
        public ActionResult DeletePost(int model)
        {
            RegionalPostModel mode = new RegionalPostModel();
            mode.Delete_ExamPost(model);
            return Json(new { tableData = mode.ListRegionalPost }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult DeletePostRule(int model,string postname,string departcode)
        {
            RegionalPostModel mode = new RegionalPostModel();
            mode.DeleteExamPostRule(model, postname, departcode);
            return Json(new { PostRuleList = mode.ListExamPostRule }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult SearchAuditUserByType(string subject)
        {
            SupervisorAuditModel model = new SupervisorAuditModel();
            var name = this.UserNameContext;
            model.GetAllExamUserByType(subject,name);
            var IsSuper = model.LSuperExamType.Count() != 0 ||model.Ladvtuserstype .Count()!=0? true : false;
            return Json(new { LCheckAudtiUser = model.LCheckAudtiUser, LRules = model.LRules, LSignedupUser = model.LSignedupUser, LExamType = model.LExamType,model.LSuperExamType,model.LRecordupUser, IsSuper });
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult CelarQuata(ExamUserDetailInfo model)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            var username = this.UserNameContext;
            models.CelarQuata(model,username);
            return Json(new { LRecordupUser = models.LRecordupUser });
        }
        [MyAuthorize]
        public ActionResult CelarUpLevel(string model)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            models.TypeSubject(model);
            return Json(new { LSubject = models.LSubject });
        }
        [MyAuthorize]
        public ActionResult SaveUpLevel(ExamUserDetailInfo model,string newsubject)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            var username = this.UserNameContext;
            var result=models.SaveUpLevel(model,newsubject, username);
            return Json(new {Result= result, LRecordupUser = models.LRecordupUser });
        }
       [MyAuthorize]
        public ActionResult AduitSigupBySubject(string usercode)
        {
            SupervisorAuditModel model = new SupervisorAuditModel();
            model.GetSignSubject(usercode);
            return Json(new { LExamSubject = model.LExamSubject });
        }
        [MyAuthorize]
        public ActionResult GetProcessUser(SearchHrData data)
        {
            HrAuditModel models = new HrAuditModel();
            models.GetProcessUser(data);
            return Json(new { models.ListProcessUser });
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult StopProUser(string model, SearchHrData data)
        {
            HrAuditModel models = new HrAuditModel();
            var username = this.UserNameContext;
            models.StopProUser(model, username, data);
            return Json(new { models.ListProcessUser });
        }
        //[HttpGet]
        public string SendWeiXin()
        {
            var resu = string.Empty;
            HrAuditModel models = new HrAuditModel();
            resu = models.WeiXinJob();
            return resu;
        }

    }
}
