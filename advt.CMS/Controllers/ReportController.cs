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
using advt.CMS.Models.ExamModel;
using advt.CMS.Models;

namespace advt.Web.Controllers
{
    public class ReportController : BaseController
    {
        [MyAuthorize]
        public ActionResult ExamDataReport()
        {
            ExamDataModel model = new ExamDataModel();
            return View(model);
        }
        public ActionResult GetExamInfo(string typename)
        {
            ExamDataModel model = new ExamDataModel();
            model.GetExamInfo(typename);
            return Json(new { ListElectronicUserView =model.ListElectronicUserView }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExamByTypeName(string typename)
        {
            ExamDataModel model = new ExamDataModel();
            model.GetExamByTypeName(typename);
            return Json(new { ListElectronicUser = model.ListElectronicUser }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetExamUserBySubjectName(string SearchData)
        {
            ExamDataModel model = new ExamDataModel();
            model.GetExamUserBySubjectName(SearchData);
            return Json(new { ListUsers = model.ListUsers }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSigupElectronicUser(string usercode, string UserCostCenter, string SubjectName, string sdata,string typename)
        {
            ExamDataModel model = new ExamDataModel();
            var username = this.UserNameContext;
            var success=model.GetSigupElectronicUser(usercode, UserCostCenter, SubjectName, sdata, typename, username);
            return Json(new { ListUsers = model.ListUsers, success, ListElectronicUserView = model.ListElectronicUserView }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteElectronicUser(string ID,string SubjectNames,string typename)
        {
            ExamDataModel model = new ExamDataModel();
            var username = this.UserNameContext;
            var success = model.DeleteElectronicUser(ID, SubjectNames, typename, username);
            return Json(new { ListElectronicUser = model.ListElectronicUser, success, ListElectronicUserView = model.ListElectronicUserView }, JsonRequestBehavior.AllowGet);
        }
        

    }
}
