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
using System.ServiceModel.Syndication;

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
                    model.ExamFailResult += model.GetExamBankNum(RuleName, usercode.UserCode);
                    if (IsTest == "formal")
                    {
                        if(detail.ExamDate == null)
                        {
                            model.ExamFailResult += "未维护考试时间:" + detail.ExamDate + ",不可考试";
                        }
                        else
                        {
                            if (detail.IsStartExam)
                            {
                                var ddate = DateTime.Now.ToString("d");
                                var examdetail = Convert.ToDateTime(detail.ExamDate).ToString("d");
                                var examendtime = string.Format("{0}/{1}/{2} {3}:{4}", Convert.ToDateTime(detail.ExamDate).Year, Convert.ToDateTime(detail.ExamDate).Month, Convert.ToDateTime(detail.ExamDate).Day, "20", "00");
                                if (ddate != examdetail)
                                {
                                    model.ExamFailResult += "不符合考试时间:" + detail.ExamDate + ",不可考试";
                                }
                                else
                                {
                                    if (DateTime.Now < detail.ExamDate || DateTime.Now > Convert.ToDateTime(examendtime))
                                    {
                                        model.ExamFailResult += "不符合考试时间:" + detail.ExamDate + ",不可考试";
                                    }
                                }
                            }
                            else
                            { model.ExamFailResult += "未点名签到"; }
                           

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
                    var set = Data.ExamScore.Get_All_ExamScore(new { CreateUser = model.VExamUserInfo.UserName, IsTest = false, ExamSubject = model.VExamUserInfo.ExamSubject, ExamType = model.VExamUserInfo.ExamType });
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

        //签到点名页面
        [MyAuthorize]
        public ActionResult ExamConfirm()
        {
            ExamConfirmModel model = new ExamConfirmModel();
            return View(model);
        }

        [MyAuthorize]
        public ActionResult ExamConfirmInfo(SearchHrData data)
        {
            ExamConfirmModel model = new ExamConfirmModel();
            var name = this.UserNameContext;
            model.GetExamInfo(data);
            return Json(new { tableData = model.LstUserInfos, model.ListWorkPlace,model.LExamType }, JsonRequestBehavior.AllowGet);
        }
      
        [MyAuthorize]
        public ActionResult SaveConfirmUser(int model, SearchHrData data)
        {
            ExamConfirmModel models = new ExamConfirmModel();
            var username = this.UserNameContext;
            models.SaveConfirmUser(model, username);
            models.GetExamInfo(data);
            return Json(new { tableData = models.LstUserInfos }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetAdminType()
        {
            ExamConfirmModel model = new ExamConfirmModel();
            var name = this.UserNameContext;
            bool isadmin=model.GetAdminType(name);
            return Json(new { tableData = isadmin }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetOrgDepartName(string model)
        {
            ExamConfirmModel models = new ExamConfirmModel();
            models.GetDepartcode(model);
            return Json(new {  models.LDepartCode }, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult GetTypeSubjectName(string model)
        {
            ExamConfirmModel models = new ExamConfirmModel();
            models.GetTypeSubject(model);
            return Json(new { models.LSubject }, JsonRequestBehavior.AllowGet);
        }

        public FileResult HrExportUser(string WrokPlace,string DepartCode,string UserCode,string ExamDate, string PostID ,string TypeName ,string  ExamProcess )
        {
            SearchHrData data = new SearchHrData();
            data.WrokPlace = WrokPlace== "undefined"?null: WrokPlace;
            data.DepartCode = DepartCode == "undefined" ? null: DepartCode;
            data.UserCode = UserCode == "undefined" ? null : UserCode;
            data.PostID = PostID == "undefined" ? null : PostID;
            data.TypeName = TypeName == "undefined" ? null : TypeName;
            data.ExamProcess = ExamProcess == "undefined" ? null : ExamProcess;
            data.ExamDate = ExamDate == "undefined" ? null : ExamDate;

            try
            {
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                HrAuditModel models = new HrAuditModel();
                models.GetProcessUser(data);
                //获取list数据
                var tlst = models.ListProcessUser;
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("考试类型");
                row1.CreateCell(1).SetCellValue("区域");
                row1.CreateCell(2).SetCellValue("部门代码");
                row1.CreateCell(3).SetCellValue("工号");
                row1.CreateCell(4).SetCellValue("姓名");
                row1.CreateCell(5).SetCellValue("岗位");
                row1.CreateCell(6).SetCellValue("考试进度");
                row1.CreateCell(7).SetCellValue("考试科目");
                row1.CreateCell(8).SetCellValue("Hr确认时间");
                row1.CreateCell(9).SetCellValue("考试日期");
                //将数据逐步写入sheet1各个行
                for (int i = 0; i < tlst.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(tlst[i].TypeName);
                    rowtemp.CreateCell(1).SetCellValue(tlst[i].WrokPlace);
                    rowtemp.CreateCell(2).SetCellValue(tlst[i].DepartCode);
                    rowtemp.CreateCell(3).SetCellValue(tlst[i].UserCode);
                    rowtemp.CreateCell(4).SetCellValue(tlst[i].UserName);
                    rowtemp.CreateCell(5).SetCellValue(tlst[i].PostID);
                    rowtemp.CreateCell(6).SetCellValue(tlst[i].ExamProcess);
                    rowtemp.CreateCell(7).SetCellValue(tlst[i].SubjectName);
                    rowtemp.CreateCell(8).SetCellValue(tlst[i].HrCheckDate);
                    rowtemp.CreateCell(9).SetCellValue(tlst[i].ExamDate);
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "HR审核考试进度明细" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception)
            {
                throw;
            }

        }
        public FileResult ExportConfirmUser(string WrokPlace, string DepartCode, string UserCode, string ExamDate, string UserName, string TypeName, string SubjectName)
        {
            SearchHrData data = new SearchHrData();
            data.WrokPlace = WrokPlace == "undefined" ? null : WrokPlace;
            data.DepartCode = DepartCode == "undefined" ? null : DepartCode;
            data.UserCode = UserCode == "undefined" ? null : UserCode;
            data.UserName = UserName == "undefined" ? null : UserName;
            data.TypeName = TypeName == "undefined" ? null : TypeName;
            data.SubjectName = SubjectName == "undefined" ? null : SubjectName;
            data.ExamDate = ExamDate == "undefined" ? null :ExamDate;

            try
            {
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                ExamConfirmModel models = new ExamConfirmModel();
                models.GetExamInfo(data);
                //获取list数据
                var tlst = models.LstUserInfos;
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("区域");
                row1.CreateCell(1).SetCellValue("部门");
                row1.CreateCell(2).SetCellValue("工号");
                row1.CreateCell(3).SetCellValue("姓名");
                row1.CreateCell(4).SetCellValue("考试类型");
                row1.CreateCell(5).SetCellValue("科目");
               
                //将数据逐步写入sheet1各个行
                for (int i = 0; i < tlst.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(tlst[i].OrgName);
                    rowtemp.CreateCell(1).SetCellValue(tlst[i].DepartCode);
                    rowtemp.CreateCell(2).SetCellValue(tlst[i].UserCode);
                    rowtemp.CreateCell(3).SetCellValue(tlst[i].UserName);
                    rowtemp.CreateCell(4).SetCellValue(tlst[i].TypeName);
                    rowtemp.CreateCell(5).SetCellValue(tlst[i].SubjectName);
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", "签到明细" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
