﻿using advt.Entity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class HrAuditModel
    {
        public string Result { get; set; }
        public List<ExamUserDetailInfo> ListHrAuditUser { get; set; }
        public List<ExamUserDetailInfo> ListHrAuditSuccessUser { get; set; }
        public List<ExamUserDetailInfo> LExamUserDetailInfo { get; set; }
        public List<KeyValuePair<string, string>> LExamType { get; set; }
        public List<PracticeInfo> LPracticeInfo { get; set; }
        public List<PracticeInfo> ListPracticeInfo { get; set; }
        public List<SearchHrData> ListProcessUser { get; set; }
        public List<string> ListWorkPlace { get; set; }
        public List<string> ListOrgName { get; set; }
        public HrAuditModel() : base()
        {
            ListHrAuditUser = new List<ExamUserDetailInfo>();
            ListHrAuditSuccessUser = new List<ExamUserDetailInfo>();
            LExamUserDetailInfo = new List<ExamUserDetailInfo>();
            LPracticeInfo = new List<PracticeInfo>();
            LExamType = new List<KeyValuePair<string, string>>();
            ListPracticeInfo = new List<PracticeInfo>();
            ListProcessUser = new List<SearchHrData>();
            ListWorkPlace = new List<string>();
            ListOrgName = new List<string>();
        }
        public void GetHrAuditUser(SearchHrData model=null)
        {
            try
            {
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(model.TypeName) || !string.IsNullOrEmpty(model.UserCode) || !string.IsNullOrEmpty(model.SubjectName) || !string.IsNullOrEmpty(model.DepartCode))
                    {

                        ListHrAuditSuccessUser = Data.ExamUserDetailInfo.Get_All_ExamUserCheckDetail(model.TypeName, model.UserCode, model.SubjectName, model.DepartCode).OrderByDescending(x => x.TypeName).ToList();
                        ListHrAuditUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup", TypeName = model.TypeName, IsExam = "false", IsStop = false }).OrderByDescending(x => x.TypeName).ToList();
                    }
                    else
                    {
                        ListHrAuditUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup", IsExam = "false", IsStop = false }).OrderByDescending(x => x.TypeName).ToList();
                        ListHrAuditSuccessUser = Data.ExamUserDetailInfo.Get_All_ExamUserCheckDetail("", "", "", "").OrderByDescending(x => x.TypeName).ToList();
                    }
                }
                else
                {
                    ListHrAuditUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup", IsExam = "false", IsStop = false }).OrderByDescending(x => x.TypeName).ToList();
                    ListHrAuditSuccessUser = Data.ExamUserDetailInfo.Get_All_ExamUserCheckDetail("", "", "", "").OrderByDescending(x => x.TypeName).ToList();

                }
                LExamType.Add(new KeyValuePair<string, string>("", "-全部-"));
                var ex = Data.ExamType.Get_All_ExamType().Select(y => new KeyValuePair<string, string>(y.TypeName,
                    y.TypeName));
                LExamType.AddRange(ex);
                var user = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo();
                ListWorkPlace = user.GroupBy(x => x.WorkPlace).Select(y=>y.Key).Distinct().ToList();
                ListOrgName = user.GroupBy(x => x.OrgName).Select(y => y.Key).Distinct().ToList();
                //foreach (var item in Data.ExamType.Get_All_ExamType())
                //{
                //    LExamType.Add(new KeyValuePair<string, string>(item.TypeName, item.TypeName));
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public void UpdateHrAduitUser(List<ExamUserDetailInfo> model, string username)
        {
            try
            {
                foreach (var items in model)
                {
                    var item = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { ID = items.ID });
                    items.HrCheckCreateUser = username;
                    items.HrCheckCreateDate = DateTime.Now;
                    items.HrCreateDate = item.HrCreateDate;
                    items.DirectorCreateDate = item.DirectorCreateDate;
                    Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(items, null, new string[] { "ID" });
                }
                //EmailHelper v = new EmailHelper();
                //v.SendEmail();
                GetHrAuditUser();
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public void StopHrAuditUser(string id, string username)
        {
            try
            {
                var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { ID = id });
                c.IsStop = true;
                c.StopCreateDate = DateTime.Now;
                c.StopCreateUser = username;
                Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(c, null, new string[] { "ID" });
                GetHrAuditUser();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //StopProUser
        public void StopProUser(string id, string username, SearchHrData data = null)
        {
            try
            {
                var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { ID = id });
                c.IsStop = true;
                c.StopCreateDate = DateTime.Now;
                c.StopCreateUser = username;
                Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(c, null, new string[] { "ID" });
                GetProcessUser(data);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void SearchPracticeUserDetail(string code)
        {
            LExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = code });
            LPracticeInfo = Data.PracticeInfo.Get_All_PracticeInfo(new { UserCode = code }).OrderByDescending(x => x.CreateDate).ToList();
        }
        public void Qualification(string filepath, string username)
        {
            DataTable dt = new DataTable();
            FileStream files = null;
            IWorkbook Workbook = null;
            var LDetail = new List<ExamUserDetailInfo>();
            try
            {

                using (files = new FileStream(filepath, FileMode.Open, FileAccess.Read))//C#文件流读取文件
                {
                    if (filepath.IndexOf(".xlsx") > 0)
                        //把xlsx文件中的数据写入Workbook中
                        Workbook = new XSSFWorkbook(files);

                    else if (filepath.IndexOf(".xls") > 0)
                        //把xls文件中的数据写入Workbook中
                        Workbook = new HSSFWorkbook(files);

                    if (Workbook != null)
                    {
                        ISheet sheet = Workbook.GetSheetAt(0);//读取第一个sheet
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        //得到Excel工作表的行 
                        IRow headerRow = sheet.GetRow(0);
                        //得到Excel工作表的总列数  
                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            //得到Excel工作表指定行的单元格  
                            ICell cell = headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();
                            if (row != null)
                            {
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                        dataRow[j] = row.GetCell(j);
                                }
                                dt.Rows.Add(dataRow);
                            }

                        }
                    }
                }
                using (var ds = dt)
                {
                    var data = new ExamUserDetailInfo();
                    foreach (DataRow item in ds.Rows)
                    {
                        if (item != null)
                        {
                            LDetail.Add(new ExamUserDetailInfo
                            {
                                TypeName = item[0].ToString().Trim(),
                                SubjectName = item[1].ToString().Trim(),
                                UserCode = item[2].ToString().Trim(),
                                UserName = item[3].ToString().Trim(),
                                DepartCode = item[4].ToString().Trim(),
                                RuleName = item[5].ToString().Trim(),
                                ExamDate = Convert.ToDateTime(item[6].ToString()),
                                ExamPlace = item[7].ToString().Trim(),
                                HrCheckCreateUser = username,
                                HrCheckCreateDate = DateTime.Now,
                                IsExam = "false",
                                IsStop = false,
                                ExamStatus = "HrCheck"
                            });
                        }
                    }
                }
                var successcount = 0;
                foreach (var item in LDetail)
                {
                    var d = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { UserCode = item.UserCode, TypeName = item.TypeName, SubjectName = item.SubjectName, IsStop = false, IsExam=false });

                    if (d != null)
                    {
                        d.IsStop = true;
                        Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(d, null, new string[] { "ID" });
                    }
                    item.IsStop = false;
                    var c = Data.ExamUserDetailInfo.Insert_ExamUserDetailInfo(item, null, new string[] { "ID" });
                    successcount += c;

                }
                GetHrAuditUser();
                Result =  "成功插入" + successcount + "记录";

            }

            catch (Exception ex)
            {
                Result = ex.Message;
                files.Close();//关闭当前流并释放资源
            }
        }

        public void UploadPractice(string filepath, string username)
        {
            DataTable dt = new DataTable();
            FileStream files = null;
            IWorkbook Workbook = null;
            var LDetail = new List<PracticeInfo>();
            try
            {

                using (files = new FileStream(filepath, FileMode.Open, FileAccess.Read))//C#文件流读取文件
                {
                    if (filepath.IndexOf(".xlsx") > 0)
                        //把xlsx文件中的数据写入Workbook中
                        Workbook = new XSSFWorkbook(files);

                    else if (filepath.IndexOf(".xls") > 0)
                        //把xls文件中的数据写入Workbook中
                        Workbook = new HSSFWorkbook(files);

                    if (Workbook != null)
                    {
                        ISheet sheet = Workbook.GetSheetAt(0);//读取第一个sheet
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        //得到Excel工作表的行 
                        IRow headerRow = sheet.GetRow(0);
                        //得到Excel工作表的总列数  
                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            //得到Excel工作表指定行的单元格  
                            ICell cell = headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();
                            if (row != null)
                            {
                                for (int j = row.FirstCellNum; j < cellCount; j++)
                                {
                                    if (row.GetCell(j) != null)
                                        dataRow[j] = row.GetCell(j);
                                }
                                dt.Rows.Add(dataRow);
                            }
                        }
                    }
                }
                using (var ds = dt)
                {
                    foreach (DataRow item in ds.Rows)
                    {
                        if (item != null)
                        {
                            LDetail.Add(new PracticeInfo
                            {
                                TypeName = item[0].ToString().Trim(),
                                SubjectName = item[1].ToString().Trim(),
                                UserCode = item[2].ToString().Trim(),
                                UserName = item[3].ToString().Trim(),
                                PracticeScore =Convert.ToDecimal(item[4].ToString().Trim()),
                                SkillName = item[5].ToString().Trim()
                            });
                        }
                    }
                }
                foreach (var item in LDetail)
                {
                    var subject = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { SubjectName = item.SubjectName, TypeName =item.TypeName, UserCode =item.UserCode, IsStop = false, IsExam = "false" });
                    if (subject != null)
                    {
                        item.CreateDate = DateTime.Now;
                        item.CreateUser = username;
                        item.ValidityDate = DateTime.Now;
                        Data.PracticeInfo.Insert_PracticeInfo(item, null, new string[] { "ID" });
                    } 
                }
                GetHrAuditUser();
                Result = "成功插入";
            }

            catch (Exception ex)
            {
                Result = ex.Message;
                files.Close();//关闭当前流并释放资源
            }
        }

        public void GetProcessUser(SearchHrData data)
        {
            var model = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { IsStop =false, IsExam=false});
            if (!string.IsNullOrEmpty(data.WrokPlace))
            {
                model = model.Where(x => x.WorkPlace == data.WrokPlace).ToList();
            }
            if (!string.IsNullOrEmpty(data.DepartCode))
            {
                model = model.Where(x => x.DepartCode == data.DepartCode).ToList();
            }
            if (!string.IsNullOrEmpty(data.UserCode))
            {
                model = model.Where(x => x.UserCode == data.UserCode).ToList();
            }
            if (!string.IsNullOrEmpty(data.PostID))
            {
                model = model.Where(x => x.PostID == data.PostID).ToList();
            }
            if (!string.IsNullOrEmpty(data.OrgName))
            {
                model = model.Where(x => x.OrgName == data.OrgName).ToList();
            }
            if (!string.IsNullOrEmpty(data.ExamDate))
            {
                var year = Convert.ToDateTime(data.ExamDate).Year;
                var month = Convert.ToDateTime(data.ExamDate).Month;
                //2022-04-30T16:00:00.000Z
                
                var dt = year + "-" + month;
                
                model = model.Where(x => x.ExamDate!=null).ToList();
                model = model.Where(x => x.ExamDate.Value.Year == year && x.ExamDate.Value.Month == month).ToList();
            }
            if (!string.IsNullOrEmpty(data.TypeName))
            {
                model = model.Where(x => x.TypeName == data.TypeName).ToList();
            }
            if (data.ExamProcess == "待主管处理")
            {
                model = model.Where(x => x.ExamStatus == "HrSignUp"&&x.IsExam== "false").ToList();
            }
            else if (data.ExamProcess == "待HR审核")
            {
                model = model.Where(x => x.ExamStatus == "SignUp" && x.IsExam == "false").ToList();
            }
            else if (data.ExamProcess == "待考试")
            {
                model = model.Where(x => x.ExamStatus == "HrCheck" && x.IsExam == "false").ToList();
            }
            else if (data.ExamProcess == "考试完成")
            {
                var mm = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { IsStop = false });
                model = mm.Where(x =>  x.IsExam == "true").ToList();
            }
            ListProcessUser = model.Select(y => new SearchHrData
            {
                TypeName = y.TypeName,
                WrokPlace = y.WorkPlace,
                DepartCode = y.DepartCode,
                UserCode = y.UserCode,
                ExamDate = y.ExamDate.ToString(),
                ExamProcess = GetProcess(y.ExamStatus),
                SubjectName = y.SubjectName,
                UserName= y.UserName,
                PostID=y.PostID,
                HrCheckDate = y.HrCheckCreateDate.ToString(),
                ID = y.ID,
                OrgName=y.OrgName
            }).OrderByDescending(t=>t.ExamDate).ToList();
        }
        public string GetProcess(string data)
        {
            if (data == "HrSignUp")
            {
                return "待主管处理";
            }
            else if (data == "SignUp")
            {
                return "待HR审核";
            }
            else if (data == "HrCheck")
            {
                return "待考试";
            }
            return "";
        }
    }

    public class SearchHrData
    {
        public string TypeName { get; set; }//考试类型
        public string UserCode { get; set; }//人员工号
        public string SubjectName { get; set; }//科目名称
        public string DepartCode { get; set; }//部门代码
        public string PostID { get; set; }//岗位
        public string ExamProcess { get; set; }//考试进度
        public string WrokPlace { get; set; }//工作地点
        public string OrgName { get; set; }//部门
        public string ExamDate { get; set; }//考试时间
        public string UserName { get; set; }
        public string HrCheckDate { get; set; }
        public int ID { get; set; }
    }

  
}