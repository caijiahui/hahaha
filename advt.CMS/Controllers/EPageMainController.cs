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
    public class EPageMainController : BaseController
    {
        [MyAuthorize]
        public ActionResult MaintainExam()
        {
            MaintainExamModel model = new MaintainExamModel();
            return View(model);
        }
        [MyAuthorize]
        public ActionResult MaintainExamPC()
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
        public ActionResult MaintainSubset()
        {
            var model = new ExamUserSubject();
            return View(model);
        }
        //个人考试科目
        [MyAuthorize]
        public JsonResult GetUserSubject()
        {
            var model = new ExamUserSubject();
            var username = this.UserNameContext;
            model.GetListUsersubject(username);
            return Json(new { ListUsersubject = model.ListUsersubject, username, model.usercode }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [MyAuthorize]
        public ActionResult ExamPage(string IsTest = null, string RuleName = null)
        {
            var model = new ExamPageModel();
            model.IsTest = IsTest;
            model.RuleName = RuleName;
            var username = this.UserNameContext;
            var usercode = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { EamilUsername = username });
            if (usercode != null)
            {
                var detail = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { UserCode = usercode.UserCode, RuleName = RuleName, IsStop = false, IsExam = false });
                if (detail != null)
                {
                    if (IsTest == "formal")
                    {
                        var dtdate = DateTime.Now;
                        if(detail.ExamDate == null)
                        {
                            model.ExamFailResult += "未维护考试时间:" + detail.ExamDate + ",不可考试";
                        }
                        else
                        {
                            var ddate = Convert.ToDateTime(detail.ExamDate);

                            model.ExamFailResult += model.GetExamBankNum(RuleName, usercode.UserCode);
                            if (dtdate <= ddate || detail.ExamDate == null)
                            {
                                model.ExamFailResult += "不符合考试时间:" + detail.ExamDate + ",不可考试";
                            }
                        }

                    }
                    
                }
                else
                {
                    model.ExamFailResult += "没有考试资格";
                }
            }
            return View(model);
        }
        [MyAuthorize]
        public ActionResult GetUserScore()
        {
            MaintainExamModel model = new MaintainExamModel();
            var username = this.UserNameContext;
            var code = model.GetUserCode(username);
            model.GetUserScore(code);

            return Json(new { tableData = model.ListExamScore, Name = model.Name, UserCode = model.UserCode }, JsonRequestBehavior.AllowGet);

        }
        [MyAuthorize]
        public ActionResult ExamGetFinishInfo(string ID)
        {
            ExamFinishModel model = new ExamFinishModel();
            var username = this.UserNameContext;
            var examguid = model.GetExamIDInfo(ID);
            model.GetExamListInfo(Convert.ToInt32(ID), username,"", examguid);
            model.GetCsore(examguid);

            return Json(new { VExamScore = model.VExamScore, examList = model.examList }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult InsertScore(ExamPageModel model)
        {
            try
            {
                var examguid = ""; 
                model.GetExam();
                if (model.VExamUserInfo.IsTest == false)
                {
                    var set = Data.ExamScore.Get_All_ExamScore(new { CreateUser = model.VExamUserInfo.UserName, IsTest = false, ExamSubject = model.VExamUserInfo.ExamSubject });
                    if (set.Count() == 0)
                    {                       
                        examguid = model.InsertScoreDatas(model);
                        var name = this.UserNameContext;
                        model.UpdateLevel(model, name, examguid);
                    }
                    else
                    {
                        var date = set.FirstOrDefault().CreateDate;
                        DateTime da = DateTime.Now;
                        TimeSpan ts = da.Subtract(Convert.ToDateTime(date)).Duration();
                        if (ts.TotalMinutes > 10)
                        {
                            examguid = model.InsertScoreDatas(model);
                            var name = this.UserNameContext;
                            model.UpdateLevel(model, name, examguid);
                        }


                    }
                }
                else
                {
                    examguid = model.InsertScoreDatas(model);
                }
               
                return Json(new { examList = model.Listexam, examid = examguid, VExamScore = model.VExamScore }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var TestModel = new advt_Attachment();
                TestModel.name = ex.Message;
                Data.advt_Attachment.Insert_advt_Attachment(TestModel, null, new string[] { "id" });
                throw;
            }
            
        }
        //考试类型
        [MyAuthorize]
        public ActionResult ExamFinish()
        {
            ExamFinishModel model = new ExamFinishModel();
            return View(model);
        }
        [MyAuthorize]
        [HttpPost]
        public ActionResult NextTopic(ExamPageModel model)
        {
            model.GetExam();
            return Json(new { examList = model.examList, ListBank = model.ListBankView, nowItem = model.nowItem, total = model.total, model.VExamUserInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult Test(string data, string RuleName)
        {
            var model = new ExamPageModel();
            var username = this.UserNameContext;
            model.GetListExam(data, RuleName, username);
            return Json(new { examList = model.examList, ListBankView = model.ListBankView, nowItem = model.nowItem, total = model.total, model.VExamUserInfo }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult ExamFinishInfo(string test,string examguid)
        {
            ExamFinishModel model = new ExamFinishModel();
            var name = this.UserNameContext;
            model.GetExamListInfo(model.ID, name, test, examguid);
            return Json(new { VExamScore = model.VExamScore, examList = model.examList }, JsonRequestBehavior.AllowGet);
        }

    }
}
