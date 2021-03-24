using System;
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
            return Json(new { model.LexamType,model.Result }, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetSubjectInfo()
        {
            var subjectInfo = Data.ExamSubject.Get_All_ExamSubject();
            ExamSubjectModel model = new ExamSubjectModel();
            return Json(new { tableData = subjectInfo, VexamSubject= model.VexamSubject }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetSubjectList(string model)
        {
            ExamRuleModel models = new ExamRuleModel();
            var subject = Data.ExamSubject.GetSubjectList();
            models.GetRuleType(model);
            return Json(new { ListTypeName = subject, RuleGrList = models.RuleTopicList }, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetRuleInfo()
        {
            var subjectInfo = Data.ExamRule.Get_All_ExamRule();
            ExamRuleModel model = new ExamRuleModel();
            return Json(new { tableData = subjectInfo, VExamRule = model.VExamRule }, JsonRequestBehavior.AllowGet);
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
            var Result= model.SaveRuleInfo(username);
            model.SaveTopicInfo(model.ListExamRule.LastOrDefault().ID);
            return Json(new { Result,tableData = model.ListExamRule, RuleGrList =model.RuleGrList}, JsonRequestBehavior.AllowGet);
          
        }
        [MyAuthorize]
        public ActionResult DeleteRuleInfo(int model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.Delete_ExamRule(model);
            return Json(new { tableData = models.ListExamRule }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult MaintainExamBank()
        {
            var model = new ExamBankModel();
            return View();
        }
        [MyAuthorize]
        public ActionResult GetBankInfo(string ExamType)
        {
            var model = new ExamBankModel();
            model.GetBankInfo(ExamType);
            return Json(new { LExamBank = model.LExamBank, LExamType =model.LExamType }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetTopic(int ID)
        {
            var model = new ExamBankModel();
            model.GetTopic(ID);
            return Json(new { VExamBank = model.VExamBank}, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult DeleteBankInfo(int ID)
        {
            var model = new ExamBankModel();
            model.DeleteBankInfo(ID);
            return Json(new { Result="Pass",model.LExamBank}, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult SaveBankInfo(ExamBankModel model)
        {
            var username = this.UserNameContext;
            model.SaveBankInfo(username);
            return Json(new { model.LExamBank}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Upload_TEL_MASTER(HttpPostedFileBase file)
        {
            string filepath = "";
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
            model.UploadBank(filepath);

            return Json(new {Result=model.Result, model.LExamBank }, JsonRequestBehavior.AllowGet);
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
        public ActionResult ExamPage(string IsTest=null, string RuleName=null,string ExamDate=null)
        {
            var model = new ExamPageModel();
            model.IsTest = IsTest;
            model.RuleName = RuleName;
            var dtdate = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(ExamDate))
            {
                ExamDate =Convert.ToDateTime(ExamDate).ToString("yyyy-MM-dd");
            }
            if(string.IsNullOrEmpty(ExamDate) || dtdate!= ExamDate)
            {
                model.IsExam = "false";
            }
            else
            {
                model.IsExam = "true";
            }
            return View(model);
        }
        [MyAuthorize]
        public ActionResult Test(string data,string RuleName)
        {
            var model = new ExamPageModel();
            var username = this.UserNameContext;
            model.GetListExam(data, RuleName, username);
            return Json(new { examList = model.examList, ListBankView = model.ListBankView, nowItem = model.nowItem, total = model.total, model.VExamUserInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult NextTopic(ExamPageModel model)
        {
            model.GetExam();
            return Json(new { examList = model.examList, ListBank = model.ListBankView, nowItem = model.nowItem, total = model.total, model.VExamUserInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult FinishTopic(ExamPageModel model)
        {
            model.GetExam();
            return Json(new { examList = model.examList, ListBank = model.ListBankView, nowItem = model.nowItem, total = model.total }, JsonRequestBehavior.AllowGet);
        }

        [MyAuthorize]
        [HttpPost]
        public ActionResult InsertScore(ExamPageModel model)
        {
            model.GetExam();
            model.InsertScoreData(model);
            model.InsertRecoredData(model);
            return Json(new { examList = model}, JsonRequestBehavior.AllowGet);
        }
        //考试类型
        [MyAuthorize]
        public ActionResult ExamFinish()
        {
            ExamFinishModel model = new ExamFinishModel();
            return View(model);
        }

        [MyAuthorize]
        public ActionResult ExamFinishInfo()
        {
            ExamFinishModel model = new ExamFinishModel();
            model.GetExamListInfo(model.ID);
            return Json(new { VExamScore = model.VExamScore, examList = model.examList }, JsonRequestBehavior.AllowGet);
        }

        //考试个人基本信息
        [MyAuthorize]
        public ActionResult MaintainExamUserInfo()
        {
            ExamUserInfoModel model = new ExamUserInfoModel();
            return View(model);
        }
        [MyAuthorize]
        public ActionResult GetUserInfo(string typename)
        {
            ExamUserInfoModel model = new ExamUserInfoModel();
            model.GetUserInfo(typename);
            model.GetUserComInfo();

            return Json(new { tableData = model.ListUserInfo11, YListUserInfo = model.YListUserInfo, CPListUserInfo = model.ListDetailInfo, LExamType=model.LExamType }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Upload_UserAch(HttpPostedFileBase file)
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
            model.UploadUser(filepath);
            model.GetUserInfo(null);
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
        public ActionResult InsertUserDetailInfo(List<UserInfo> model)
        {
            var username = this.UserNameContext;
            ExamUserInfoModel models = new ExamUserInfoModel();
            var result=models.InsertUserDetail(model, username);
            models.GetUserInfo(null);
            models.GetUserComInfo();
            return Json(new { tableData = models.ListUserInfo, YListUserInfo = models.YListUserInfo, CPListUserInfo = models.ListDetailInfo, Results = result });
        }
        [MyAuthorize]
        public ActionResult DeleteExamUserInfo(int model)
        {
            ExamUserInfoModel models = new ExamUserInfoModel();
            models.DeleteExamUserInfo(model);
            models.GetUserInfo(null);
            return Json(new { tableData = models.ListUserInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult SaveVExamUserInfo(ExamUserInfoModel model)
        {
            var username = this.UserNameContext;
            model.SaveUserInfo(username);
            model.GetUserInfo(null);
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
            return Json(new { LCheckAudtiUser = model.LCheckAudtiUser, LRules= model.LRules, LSignedupUser=model.LSignedupUser });
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
        public ActionResult SavePracticeInfo(PracticeInfo model)
        {
            SupervisorAuditModel models = new SupervisorAuditModel();
            models.InsertPracticeInfo(model);
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
            return Json(new { ListExamUserDetailInfos = models.ListDirectorUserInfos,  LSignedupUser = models.LSignedupUser });
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
        public ActionResult GetHrAuditUser(string typename)
        {
            HrAuditModel model = new HrAuditModel();
            model.GetHrAuditUser(typename);
            return Json(new { ListHrAuditUser = model.ListHrAuditUser, ListHrAuditSuccessUser =model.ListHrAuditSuccessUser,LExamType=model.LExamType });
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
            return Json(new { ListHrAuditUser = models.ListHrAuditUser, ListHrAuditSuccessUser = models.ListHrAuditSuccessUser });
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

        public ActionResult GetRuleTypeName(string model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.GetSubjectList(model);
            return Json(new { ListSubjectName = models.ListExamSubject, ListTopic = models.ListTopic }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRuleSubjectName(string model)
        {
            ExamRuleModel models = new ExamRuleModel();
            models.GetRuleSubjectList(model);
            return Json(new { ListTopic = models.ListTopic }, JsonRequestBehavior.AllowGet);
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
        public FileResult ShunFengExcel()
        {

            try
            {
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                ExamUserInfoModel models = new ExamUserInfoModel();
                models.GetUserInfo(null);
                //获取list数据
                var tlst = models.ListUserInfo;
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("工号");
                row1.CreateCell(1).SetCellValue("姓名");
                row1.CreateCell(2).SetCellValue("入职日期");
                row1.CreateCell(3).SetCellValue("职等");
                row1.CreateCell(4).SetCellValue("职位");
                row1.CreateCell(5).SetCellValue("本职等对应技能等级");
                row1.CreateCell(6).SetCellValue("已经考取技能等级");
                row1.CreateCell(7).SetCellValue("最近一次理论考试成绩");
                row1.CreateCell(8).SetCellValue("最近一次通过理论时间");
                row1.CreateCell(9).SetCellValue("最近一次实践成绩");
                row1.CreateCell(10).SetCellValue("最近一次实践考核通过时间");
                row1.CreateCell(11).SetCellValue("最高可考技能等级");
                row1.CreateCell(12).SetCellValue("可申请技能等级");
                row1.CreateCell(13).SetCellValue("最近一次绩效成绩要求");
                //将数据逐步写入sheet1各个行
                for (int i = 0; i < tlst.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(tlst[i].UserCode);//工号
                    rowtemp.CreateCell(1).SetCellValue(tlst[i].UserName);//姓名
                    rowtemp.CreateCell(2).SetCellValue(tlst[i].EntryDate.ToString());//入职日期
                    rowtemp.CreateCell(3).SetCellValue(tlst[i].RankName);//职等
                    rowtemp.CreateCell(4).SetCellValue(tlst[i].PostName);//职位
                    rowtemp.CreateCell(5).SetCellValue(tlst[i].SkillLevel);//本职等对应技能等级
                    rowtemp.CreateCell(6).SetCellValue(tlst[i].CurrentSkillLevel);//已经考取技能等级
                    rowtemp.CreateCell(7).SetCellValue(tlst[i].ExamScore);//最近一次理论考试成绩
                    rowtemp.CreateCell(8).SetCellValue(tlst[i].LastExamTime.ToString());//最近一次通过理论时间
                    rowtemp.CreateCell(9).SetCellValue(tlst[i].TheoreticalAchievement.ToString());//最近一次实践成绩
                    rowtemp.CreateCell(10).SetCellValue(tlst[i].PracticeTime.ToString());//最近一次实践考核通过时间
                    rowtemp.CreateCell(11).SetCellValue(tlst[i].HighestTestSkill);//最高可考技能等级
                    rowtemp.CreateCell(12).SetCellValue(tlst[i].ApplicationLevel);//可申请技能等级
                    rowtemp.CreateCell(13).SetCellValue(tlst[i].Achievement);//最近一次绩效成绩要求    
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
        public FileResult SignUpExamExcel()
        {

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


        [MyAuthorize]
        public ActionResult MaintainExam()
        {
            MaintainExamModel model = new MaintainExamModel();
            return View(model);
        }
        [MyAuthorize]
        public ActionResult MaintainSubset()
        {
            MaintainExamModel model = new MaintainExamModel();
            return View(model);
        }
        [MyAuthorize]
        public ActionResult MaintainScore()
        {
            MaintainExamModel model = new MaintainExamModel();
            return View(model);
        }

        [MyAuthorize]
        public ActionResult GetUserScore()
        {
            MaintainExamModel model = new MaintainExamModel();
            var username = this.UserNameContext;
            var code=model.GetUserCode(username);
            model.GetUserScore(code);

            return Json(new { tableData = model.ListExamScore ,Name= model.Name,UserCode=model.UserCode}, JsonRequestBehavior.AllowGet);
          
        }

        [MyAuthorize]
        public ActionResult ExamGetFinishInfo(string ID)
        {
            ExamFinishModel model = new ExamFinishModel();
            model.GetCsore(Convert.ToInt32(ID));
            model.GetExamListInfo(Convert.ToInt32(ID));
            return Json(new { VExamScore = model.VExamScore, examList = model.examList }, JsonRequestBehavior.AllowGet);
        }
        //个人考试科目
        [MyAuthorize]
        public JsonResult GetUserSubject()
        {
            var model =new ExamUserSubject();
            var username = this.UserNameContext;
            model.GetListUsersubject(username);
            return Json(new { ListUsersubject = model.ListUsersubject, username,model.usercode}, JsonRequestBehavior.AllowGet);
        }



    }
}
