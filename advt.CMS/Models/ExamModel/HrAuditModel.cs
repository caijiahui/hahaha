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
        public HrAuditModel() : base()
        {
            ListHrAuditUser = new List<ExamUserDetailInfo>();
            ListHrAuditSuccessUser = new List<ExamUserDetailInfo>();
            LExamUserDetailInfo = new List<ExamUserDetailInfo>();
            LPracticeInfo = new List<PracticeInfo>();
            LExamType = new List<KeyValuePair<string, string>>();
        }
        public void GetHrAuditUser(SearchHrData model=null)
        {
            try
            {
                if (model != null)
                {
                    ListHrAuditSuccessUser = Data.ExamUserDetailInfo.Get_All_ExamUserCheckDetail(model.TypeName, model.UserCode, model.SubjectName, model.DepartCode).OrderByDescending(x => x.TypeName).ToList();
                    ListHrAuditUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup", TypeName = model.TypeName, IsExam = false }).OrderByDescending(x => x.TypeName).ToList();
                }
                else
                {
                    ListHrAuditUser = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "Signup", TypeName = "", IsExam = false }).OrderByDescending(x => x.TypeName).ToList();
                    ListHrAuditSuccessUser = Data.ExamUserDetailInfo.Get_All_ExamUserCheckDetail("", "", "", "").OrderByDescending(x => x.TypeName).ToList();
                }
                LExamType.Add(new KeyValuePair<string, string>("", "-全部-"));
                foreach (var item in Data.ExamType.Get_All_ExamType())
                {
                    LExamType.Add(new KeyValuePair<string, string>(item.TypeName, item.TypeName));
                }
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
                foreach (var item in model)
                {
                    item.DirectorCreateUser = username;
                    item.DirectorCreateDate = DateTime.Now;
                    Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(item, null, new string[] { "ID" });
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

    }
    public class SearchHrData
    {
        public string TypeName { get; set; }
        public string UserCode { get; set; }
        public string SubjectName { get; set; }
        public string DepartCode { get; set; }
    }
}