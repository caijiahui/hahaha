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
    public class SupervisorAuditModel
    {
        public List<UserInfo> ListDirectorUserInfos { get; set; }
        public List<ExamRule> LRules { get; set; }
        public List<ExamUserDetailInfo> LExamUserDetailInfo { get; set; }
        public List<ExamUserDetailInfo> LSignedupUser { get; set; }
        public List<PracticeInfo> LPracticeInfo { get; set; }
        public List<ExamUserDetailInfo> LCheckAudtiUser { get; set; }
        public List<KeyValuePair<string, string>> LExamType { get; set; }//考试类型
        public List<KeyValuePair<string, string>> LExamSubject { get; set; }//考试科目
        public string Result { get; set; }
        public SupervisorAuditModel() : base()
        {
            ListDirectorUserInfos = new List<UserInfo>();
            LRules = new List<ExamRule>();
            LExamUserDetailInfo = new List<ExamUserDetailInfo>();
            LPracticeInfo = new List<PracticeInfo>();
            LCheckAudtiUser = new List<ExamUserDetailInfo>();
            LSignedupUser = new List<ExamUserDetailInfo>();
            LExamSubject = new List<KeyValuePair<string, string>>();
            LExamType = new List<KeyValuePair<string, string>>();
        }
        public void GetAllExamUserDetailInfo(string username=null)
        {
            try
            {
                var code = "";
                var user = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { EamilUsername = username });
                if (user!=null)
                {
                    code = user.UserCode;
                }
                var usersheets = Data.advt_user_sheet.Get_advt_user_sheet_UserJobTitle(code);
                //Hr报名 HrSignUp   主管审核Signup  hr审核 HrCheck
                var audata= Data.ExamUserDetailInfo.Get_All_UserAduitInfo("HrSignUp",code,"");
                if (audata != null && audata.Count() != 0)
                {
                    LCheckAudtiUser.AddRange(audata);
                }
                var adata = Data.ExamUserDetailInfo.Get_All_UserAduitInfo("Signup", code,"");
                LSignedupUser.AddRange(adata);
                if (LCheckAudtiUser.Count() != 0)
                {
                    var LTypename = LCheckAudtiUser.GroupBy(x => x.TypeName).Select(y => new { typename = y.Key });
                    foreach (var item in LTypename)
                    {
                        var rule = Data.ExamRule.Get_All_TypeNameExamRule(item.typename);
                        LRules.AddRange(rule);
                    }
                }
                LCheckAudtiUser = LCheckAudtiUser.OrderByDescending(x => x.TypeName).ToList();
                //主管需要审核的人员
                LSignedupUser = LSignedupUser.OrderByDescending(x => x.TypeName).ThenBy(x => x.DepartCode).ToList();
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

        //根据考试类型筛选出可报名人员
        public void GetAllExamUserByType(string typename,string username=null)
        {
            var code = "";
            var user = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { EamilUsername = username });
            if (user != null)
            {
                code = user.UserCode;
            }
            //Hr报名 HrSignUp   主管审核Signup  hr审核 HrCheck
            var audata = Data.ExamUserDetailInfo.Get_All_UserAduitInfo("HrSignUp", code, typename);
            LCheckAudtiUser.AddRange(audata);
            var adata = Data.ExamUserDetailInfo.Get_All_UserAduitInfo("Signup", code, typename);
            LSignedupUser.AddRange(adata);
            LCheckAudtiUser = LCheckAudtiUser.OrderByDescending(x => x.DepartCode).ToList();
            //主管需要审核的人员
            LSignedupUser = LSignedupUser.OrderByDescending(x => x.TypeName).ThenBy(x => x.DepartCode).ToList();
            LExamType.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamType.Get_All_ExamType())
            {
                LExamType.Add(new KeyValuePair<string, string>(item.TypeName, item.TypeName));
            }
        }

        //根据选择的报名人员筛选出可以选择的考试科目
        public void GetSignSubject(string depart)
        {
            var ListCode = depart.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var ListCodes = ListCode.Distinct().ToList();
            var codestring = string.Empty;
            foreach (var item in ListCodes)
            {
            
               codestring += "or  a.DepartCode like '%" + item + "%' ";
               
            }
            codestring = codestring.Substring(2, codestring.Length - 2);
            var data = Data.ExamSubject.Get_All_ExamGetByDepart(codestring);
            var c = data.Select(x => new KeyValuePair<string, string>(x.SubjectName, x.TypeName));
            LExamSubject.AddRange(c);
            LExamSubject = LExamSubject.Distinct().ToList();
        }
        public void SearchPracticeInfo(string code)
        {
            LPracticeInfo = Data.PracticeInfo.Get_All_PracticeInfo(new { UserCode = code }).OrderByDescending(x=>x.CreateDate).ToList();
        }
        public void InsertPracticeInfo(PracticeInfo data,string username)
        {
            data.CreateUser = username;
            data.CreateDate = DateTime.Now;
            Data.PracticeInfo.Insert_PracticeInfo(data, null, new string[] { "ID" });
          
        }
        public string InsertUserDetail(List<ExamUserDetailInfo> data,string username)
        {
            try
            {
                var Result = "";
                var ListExamUserDetailInfos = new List<ExamUserDetailInfo>();
                foreach (var items in data)
                {
                    var item = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { ID = items.ID });
                    item.ExamStatus = "Signup";
                    var examRule = Data.ExamRule.Get_ExamRule(new { RuleName = item.RuleName });
                    if (examRule != null)
                    {
                        if (examRule.PassPracticeScore != 0)
                        {
                            var ListPract = new List<PracticeInfo>();
                            if (!string.IsNullOrEmpty(item.SubjectName))
                            {
                                ListPract = Data.PracticeInfo.Get_All_PracticeInfo(new {  TypeName = item.TypeName, SubjectName = item.SubjectName, UserCode = item.UserCode });
                            }
                            else
                            {
                                ListPract = Data.PracticeInfo.Get_All_PracticeInfo(new { SkillName = item.ApplyLevel, TypeName = item.TypeName, UserCode = item.UserCode });
                            }
                            var Pract = new PracticeInfo();
                            if (ListPract.Count() > 0)
                            {
                                Pract = ListPract.OrderByDescending(x => x.CreateDate).FirstOrDefault();
                                if (Pract.PracticeScore < examRule.PassPracticeScore)
                                {
                                    Result = item.UserName + "实践分数未达标,不可报名。考试规则要求实践分数" + examRule.PassPracticeScore + "目前实践分数" + Pract.PracticeScore;
                                }
                                if (Pract.ValidityDate != null)
                                {
                                     var da = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                                    if (Pract.ValidityDate < da)
                                    {
                                        Result = item.UserName + item.TypeName + item.SubjectName + "实践成绩已过期,请重新填写";
                                    }
                                }
                            }
                            else
                            {
                                Result = "未找到" + item.UserName + item.TypeName + item.SubjectName + "的实践成绩，不可报名";
                            }
                        }
                        
                    }
                    else
                    {
                        Result = "未找到" + item.UserName + "对应的考试规则"+item.RuleName+"，不可报名";
                    }
                    if (string.IsNullOrEmpty(Result))
                    {
                        var part = Data.PracticeInfo.Get_PracticeInfo(new { SubjectName = item.SubjectName });
                        item.DirectorCreateDate = DateTime.Now;
                        item.DirectorCreateUser = username;
                        item.RuleName = item.RuleName;
                        Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(item, null, new string[] { "ID" });
                    }
                    

                };
                GetAllExamUserDetailInfo(username);
                return Result;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void SerachDetailByUserCode(string Code)
        {
            LExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = Code });
            
        }
        public void Stopuser(string ID,string username)
        {
            try
            {
                var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { ID = ID });
                c.IsStop = true;
                c.StopCreateDate = DateTime.Now;
                c.StopCreateUser = username;
                var code = "";
                var typename = c.TypeName;
                var user = Data.ExamUsersFromehr.Get_ExamUsersFromehr(new { EamilUsername = username });
                if (user != null)
                {
                    code = user.UserCode;
                }
                Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(c, null, new string[] { "ID" });
                GetAllExamUserByType(typename, username);
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }
        public void UploadPractice(string filepath,string username)
        {
            DataTable dt = new DataTable();
            FileStream files = null;
            IWorkbook Workbook = null;
            var LPrac = new List<PracticeInfo>();
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

                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            dt.Rows.Add(dataRow);
                        }
                    }
                }

                DateTime? v = new DateTime();
                using (var ds = dt)
                {
                    var q = from DataRow dr in ds.Rows

                            select new Entity.PracticeInfo
                            {
                                UserCode = dr[0].ToString().Trim(),
                                UserName = dr[1].ToString().Trim(),
                                ValidityDate = dr[2].ToString() != "" ? Convert.ToDateTime(dr[2].ToString()) : v,
                                PracticeScore = Convert.ToDecimal(dr[3].ToString()),
                                PracticeRemark = dr[4].ToString().Trim(),
                                SkillName = dr[5].ToString().Trim(),
                                TypeName = dr[6].ToString().Trim(),
                                SubjectName = dr[7].ToString().Trim(),
                                CreateUser=username,
                                CreateDate=DateTime.Now
                            };
                    LPrac = q.ToList();
                }
                var successcount = 0;
                foreach (var item in LPrac)
                {
                    if (item.PracticeScore != null && item.PracticeScore != 0)
                    {
                        var c = Data.PracticeInfo.Insert_PracticeInfo(item, null, new string[] { "ID" });
                        successcount += c;
                    }

                }
                Result = successcount + "成功插入" + successcount + "记录";

            }

            catch (Exception ex)
            {
                Result = ex.Message;
                files.Close();//关闭当前流并释放资源
            }
        }
    }
}