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

namespace advt.Web.Controllers
{
    public class PEMainController : BaseController
    {
        [MyAuthorize]
        public ActionResult Index(string tname,string searchdata,string startdt, string enddt, int? page,int tid =1)
        {
            tname = "Engineering";
            Model.PageModel.PEModel model = new Model.PageModel.PEModel();
            model.username = this.UserContext.username.Substring(0,this.UserContext.username.Length-17);
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
            model.username = this.UserContext.username.Substring(0, this.UserContext.username.Length - 17);
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
            V_topics.username = this.UserContext.username;
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
            model.username = this.UserContext.username.Substring(0, this.UserContext.username.Length - 17);
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
            var username = this.UserContext.username;
            model.SaveType(username);
            return Json(new { model.LexamType }, JsonRequestBehavior.AllowGet);
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
            var username = this.UserContext.username.Substring(0, this.UserContext.username.Length - 17);
            model.SaveSubject(username);
            return Json(new { tableData = model.ListExamSubject }, JsonRequestBehavior.AllowGet);
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
            var username = this.UserContext.username.Substring(0, this.UserContext.username.Length - 17);
            model.SaveRuleInfo(username);
            model.SaveTopicInfo(model.ListExamRule.LastOrDefault().ID);
            return Json(new { tableData = model.ListExamRule, RuleGrList =model.RuleGrList}, JsonRequestBehavior.AllowGet);
          
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
            var username = this.UserContextSubstring;
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
        public ActionResult ExamPage()
        {
            var model = new ExamPageModel();
            return View(model);
        }
        [MyAuthorize]
        public ActionResult Test()
        {
            var model = new ExamPageModel();
            model.GetListExam();
            return Json(new { examList = model.examList, ListBankView = model.ListBankView, nowItem = model.nowItem, total = model.total }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult NextTopic(ExamPageModel model)
        {
            model.GetExam();
            return Json(new { examList = model.examList, ListBank = model.ListBankView, nowItem = model.nowItem, total = model.total }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult FinishTopic(ExamPageModel model)
        {
            model.GetExam();
            return Json(new { examList = model.examList, ListBank = model.ListBankView, nowItem = model.nowItem, total = model.total }, JsonRequestBehavior.AllowGet);
        }
    }
}
