﻿using advt.Entity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace advt.CMS.Models.ExamModel
{
    public class ExamUserInfoModel
    {
        public UserInfo UserInfoList { get; set; }
        public List<UserInfo> ListUserInfo { get; set; }
        public List<UserInfo> ListUserInfo11 { get; set; }
        public List<UserInfo> YListUserInfo { get; set; }
        public List<Entity.RankInfo> ListRankInfo { get; set; }
        public List<Entity.RankInfo> ListRank { get; set; }

        public List<Entity.ExamUserDetailInfo> ListExamUserDetailInfo { get; set; }
        public List<Entity.ExamUserDetailInfo> ListDetailInfo { get; set; }
        public string Result { get; set; }
        public List<Entity.PracticeInfo> ListPracticeInfo { get; set; }
        public Entity.ExamUserInfo VExamUserInfo { get; set; }
        public Entity.ExamUserDetailInfo VExamUserDetailInfo { get; set; }
        public List<Entity.ExamUserInfo> ListExamUserInfo { get; set; }
        public List<Entity.SkillInfo> ListSkillInfo { get; set; }
        public List<PracticeInfo> LPracticeInfo { get; set; }
        public List<ExamUserDetailInfo> LExamUserDetailInfo { get; set; }
        public List<AchieveRecord> LExamUserMeritsRecord { get; set; }
        public List<KeyValuePair<string, string>> LExamType { get; set; }
        public List<KeyValuePair<string, string>> LWorkPlace { get; set; }
        public string Results { get; set; }
        public bool IsHrSign { get; set; }
        public List<AchieveRecord> ListAchieveRecord { get; set; }
        public MesUserInfo MesUserInfoModel { get; set; }
        public ExamUserInfoModel() : base()
        {
            UserInfoList = new UserInfo();
            ListUserInfo = new List<UserInfo>();
            YListUserInfo = new List<UserInfo>();
            ListRankInfo = new List<Entity.RankInfo>();
            ListExamUserDetailInfo = new List<Entity.ExamUserDetailInfo>();
            ListPracticeInfo = new List<Entity.PracticeInfo>();
            VExamUserInfo = new Entity.ExamUserInfo();
            ListExamUserInfo = new List<Entity.ExamUserInfo>();
            VExamUserDetailInfo = new Entity.ExamUserDetailInfo();
            ListRank = new List<Entity.RankInfo>();
            ListSkillInfo = new List<Entity.SkillInfo>();
            LPracticeInfo = new List<PracticeInfo>();
            LExamUserDetailInfo = new List<ExamUserDetailInfo>();
            ListDetailInfo = new List<ExamUserDetailInfo>();
            LExamType = new List<KeyValuePair<string, string>>();
            ListUserInfo11 = new List<UserInfo>();
            LExamUserMeritsRecord = new List<AchieveRecord>();
            MesUserInfoModel = new MesUserInfo();
            ListAchieveRecord = new List<AchieveRecord>();



            LExamType.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamType.Get_All_ExamType())
            {
                LExamType.Add(new KeyValuePair<string, string>(item.ID.ToString(), item.TypeName));
            }
            LWorkPlace = new List<KeyValuePair<string, string>>();
            LWorkPlace.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamUserInfo.Get_All_ExamUserInfo().Where(x=>x.OrgName!=null).GroupBy(x=>x.OrgName))
            {
                LWorkPlace.Add(new KeyValuePair<string, string>(item.Key.ToString(), item.Key.ToString()));
            }
        }
        public void GetUserInfo(SearchUserData data)
        {
            var connectionString = "server=172.21.214.28;database=ExamDB;uid=ExamSa;pwd=1Ex@m2021";
            DataSet result = new DataSet();
            if (string.IsNullOrEmpty(data.TypeName))
            {
                data.TypeName = "53";
            }
            if (!string.IsNullOrEmpty(data.TypeName))
            {                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_Exam_User_Info", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ExamType", SqlDbType.NVarChar, 500);                        
                        cmd.Parameters.Add("@UserCode", SqlDbType.NVarChar, 500);
                        cmd.Parameters.Add("@DepartCode", SqlDbType.NVarChar, 500);
                        cmd.Parameters.Add("@ReadyExamDate", SqlDbType.NVarChar, 500);
                        cmd.Parameters.Add("@WorkPlace", SqlDbType.NVarChar, 500);
                        cmd.Parameters["@ExamType"].SqlValue = data.TypeName;
                        if (data.UserCode != null)
                            cmd.Parameters["@UserCode"].SqlValue = data.UserCode;
                        else
                        {
                            cmd.Parameters["@UserCode"].SqlValue = DBNull.Value;
                        }
                        if (data.DepartCode != null)
                            cmd.Parameters["@DepartCode"].SqlValue = data.DepartCode;
                        else
                        {
                            cmd.Parameters["@DepartCode"].SqlValue = DBNull.Value;
                        }
                        if (data.ReadyExamDate != null)
                            cmd.Parameters["@ReadyExamDate"].SqlValue = data.ReadyExamDate;
                        else
                        {
                            cmd.Parameters["@ReadyExamDate"].SqlValue = DBNull.Value;
                        }
                        if (data.WorkPlace != null)
                            cmd.Parameters["@WorkPlace"].SqlValue = data.WorkPlace;
                        else
                        {
                            cmd.Parameters["@WorkPlace"].SqlValue = DBNull.Value;
                        }
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(result);
                    }
                }
                if (result.Tables.Count != 0)
                {
                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        decimal score = 0;
                        DateTime? now = null;
                        DateTime? practicetime = null;
                        var ReadyExamDate = "";
                        if (row["TheoreticalAchievement"].ToString() != "")
                        {
                            score = Convert.ToDecimal(row["TheoreticalAchievement"].ToString());
                        }
                        if (row["LastExamTime"].ToString() != "")
                        {
                            now = Convert.ToDateTime(row["LastExamTime"].ToString());
                        }
                        if (row["PariceDate"].ToString() != "")
                        {
                            practicetime = Convert.ToDateTime(row["PariceDate"].ToString());
                        }
                        if (data.TypeName == "53"|| data.TypeName == "65" || data.TypeName == "66" || data.TypeName == "62")
                        {
                            if (row["ReadyExamDate"].ToString().Contains("1900"))
                            {
                                ReadyExamDate = null;
                            }
                            else
                            {
                                var re = Data.RegionalPost.Get_All_RegionalPost(new {ExamType= row["TypeName"].ToString(), DepartCode = row["DepartCode"].ToString() });
                                if (re.Count()>0&&re!=null)
                                {
                                    ReadyExamDate = row["ReadyExamDate"].ToString();
                                }
                                else ReadyExamDate = null;
                            }
                        }
                        var na = Data.advt_user_sheet.Get_All_advt_user_sheet(new { UserCode = row["UserCode"].ToString(), UserCostCenter= row["DepartCode"].ToString() });

                        var exammon = "";
                        var sub = "";
                        var rule = "";
                        var app = "";
                        if (na.Count() > 0&&na!=null)
                        {
                            //判断true,true为课长
                            var nc= Data.ExamUsersFromehr.Get_All_ExamUsersFromehr(new { UserCode= na.FirstOrDefault().UserCode,UserDept= na.FirstOrDefault().UserCostCenter, JobCode = "课长" });

                            if (!string.IsNullOrEmpty(na.FirstOrDefault().UserJobTitle)||nc.Count()>0 || row["RankName"].ToString().Contains("D"))
                            {
                                exammon = null;
                                ReadyExamDate = null;
                            }
                            else
                            { 
                                exammon = row["ExamineMonth"].ToString();
                                sub = row["SubjectName"].ToString();
                                rule = row["RuleName"].ToString();
                                app = row["examapply"].ToString();
                            } 
                        }
                        ListUserInfo.Add(new UserInfo
                        {
                            Id = Convert.ToInt32(row["ID"].ToString()),
                            TypeName = row["TypeName"].ToString(),
                            UserCode = row["UserCode"].ToString(),
                            UserName = row["UserName"].ToString(),
                            EntryDate = Convert.ToDateTime(row["EntryDate"]),
                            RankName = row["RankName"].ToString(),
                            PostName = row["PostName"].ToString(),
                            DepartCode = row["DepartCode"].ToString(),
                            SkillLevel = row["SkillLevel"].ToString(),//本职等
                            CurrentSkillLevel = row["CurrentSkillLevel"].ToString(),//已经考取技能等级  
                            ExamScore = row["ExamScore"].ToString(),//最近一次理论考试成绩
                            LastExamTime = now,//最后一次理论时间
                            TheoreticalAchievement = score,//实践成绩
                            PracticeTime = practicetime,//最后一次实践成绩
                            HighestTestSkill = row["HighestTestSkill"].ToString(),//最高可考技能
                            ApplicationLevel = app,//本次申请等级      
                            IsAchment = row["IsAchment"].ToString(),
                            Achievement = row["Achievement"].ToString(),
                            SubjectName = sub,
                            ReverseBuckle = row["ReverseBuckle"].ToString(),//最初职等
                            RuleName = rule,
                            WorkPlace = row["WorkPlace"].ToString(),
                            IsUserExam = row["IsUserExams"].ToString(),
                            //每年应复审时间段
                            ExamineMonth = exammon,
                            ReadExamDate = rule== "" ? "" : ReadyExamDate
                        });
                    }
                    ListUserInfo11 = ListUserInfo.OrderByDescending(x=>x.ReadExamDate!=null).ToList();
                   
                    var listinfo = ListUserInfo.Where(x => x.IsUserExam == "true" && !string.IsNullOrEmpty(x.RuleName));
                    if (listinfo.Count() > 0 && listinfo != null)
                    { 
                        int i = 0;
                        foreach (var item in listinfo)
                        {    
                            var dff = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { TypeName = item.TypeName, ApplyLevel = item.ApplicationLevel, UserCode = item.UserCode, UserName = item.UserName, SubjectName = item.SubjectName, RuleName = item.RuleName,  IsStop = false, IsExam = "false" });
                            if (dff.Count() == 0)
                            {
                                i++;
                                YListUserInfo.Add(new UserInfo
                                {
                                    Id = i,
                                    TypeName = item.TypeName,
                                    UserCode = item.UserCode,
                                    UserName = item.UserName,
                                    EntryDate = item.EntryDate,
                                    RankName = item.RankName,
                                    PostName = item.PostName,
                                    DepartCode = item.DepartCode,
                                    SkillLevel = item.SkillLevel,
                                    CurrentSkillLevel = item.CurrentSkillLevel,
                                    ExamScore = item.ExamScore,
                                    LastExamTime = item.LastExamTime,
                                    TheoreticalAchievement = item.TheoreticalAchievement,
                                    PracticeTime = item.PracticeTime,
                                    HighestTestSkill = item.HighestTestSkill,
                                    ApplicationLevel = item.ApplicationLevel,
                                    IsAchment = item.IsAchment,
                                    Achievement = item.Achievement,
                                    SubjectName = item.SubjectName,
                                    ReverseBuckle = item.ReverseBuckle,
                                    RuleName = item.RuleName,
                                    WorkPlace = item.WorkPlace,
                                    IsUserExam = item.IsUserExam,
                                    ExamineMonth = item.ExamineMonth,
                                    ReadExamDate =item.ReadExamDate
                                }); ;
                            }
                        }
                    }
                    var tyname = Data.ExamType.Get_All_ExamType(new { ID= data.TypeName });
                    if (!string.IsNullOrEmpty(data.DepartCode))
                    {
                        ListDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrSignUp", IsStop = false, TypeName = tyname.FirstOrDefault().TypeName, DepartCode = data.DepartCode });
                    }
                    else
                    {
                        ListDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrSignUp", IsStop = false, TypeName = tyname.FirstOrDefault().TypeName});
                    }
                }

            }
        
        }

        public void UploadUser(string filepath,string name)
        {
            DataTable dt = new DataTable();
            FileStream files = null;
            IWorkbook Workbook = null;
            var LBank = new List<Entity.ExamUserInfo>();
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
                using (var ds = dt)
                {
                    var q = from DataRow dr in ds.Rows

                            select new Entity.ExamUserInfo
                            {
                                UserCode = dr[0].ToString().Trim(),
                                Achievement = dr[1].ToString().Trim(),

                            };
                    LBank = q.ToList();
                }
                foreach (var item in LBank)
                {
                    //根据工号去更新绩效
                    Data.ExamUserInfo.Get_UpdateExamUserInfo(item.UserCode, item.Achievement);

                    var record = new AchieveRecord();
                    record.UserCode = item.UserCode;
                    record.CreateUser = name;
                    record.CreateDate = DateTime.Now;
                    record.Achievement = item.Achievement;
                    record.RecordType = "绩效";
                    Data.AchieveRecord.Insert_AchieveRecord(record,null,new string[]{ "ID"});

                    Result = "success";
                }

            }

            catch (Exception ex)
            {

                files.Close();//关闭当前流并释放资源
            }
        }

        public void GetExamUser(int id)
        {
            VExamUserInfo = Data.ExamUserInfo.Get_ExamUserInfo(id);
        }
        public void DeleteExamUserInfo(int model)
        {
            Data.ExamUserInfo.Delete_ExamUserInfo(model);
        }
        public void ReverseExamUserInfo(int model,string username)
        {
            var Level = Data.ExamUserInfo.Get_All_ExamUserInfo(new { ID = model });
            Data.ExamUserInfo.ReverseExamUserInfo(model, Level.FirstOrDefault().ApplicationLevel, username);
            var record = new AchieveRecord();
            record.UserCode = Level.FirstOrDefault().UserCode;
            record.CreateUser = username;
            record.CreateDate = DateTime.Now;
            record.RecordType = "倒扣";
            Data.AchieveRecord.Insert_AchieveRecord(record);
        }
        

        public void SaveUserInfo(string username)
        {
            if (VExamUserInfo.ID != 0)
            {
                VExamUserInfo.UpdateUser = username;
                VExamUserInfo.UpdateDate = DateTime.Now;
                Data.ExamUserInfo.Update_ExamUserInfo(VExamUserInfo, null, new string[] { "ID" });
            }
            else
            {

                VExamUserInfo.CreateUser = username;
                VExamUserInfo.CreateDate = DateTime.Now;
                
                Data.ExamUserInfo.Insert_ExamUserInfo(VExamUserInfo, null, new string[] { "ID" });

            }

        }
        public void SearchPracticeInfo(string code)
        {
            LPracticeInfo = Data.PracticeInfo.Get_All_PracticeInfo(new { UserCode = code }).OrderByDescending(x => x.CreateDate).ToList();
        }

        public void SerachDetailByUserCode(string Code)
        {
            LExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = Code });
        }
        public void SerachMeritsRecord(string Code)
        {
            LExamUserMeritsRecord = Data.AchieveRecord.Get_All_AchieveRecord(new { UserCode = Code });
        }        

        public string InsertUserDetail(List<UserInfo> data, string username)
        {
            var result = "";
            try
            {
                foreach (var item in data)
                {
                    //判断是否再继续报名

                    var listuserdatail = new List<ExamUserDetailInfo>();
                    listuserdatail = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode=item.UserCode,SubjectName=item.SubjectName,IsStop=false,ExamStatus= "HrSignUp" });
                    if (listuserdatail != null && listuserdatail.Count() > 0)
                    {
                        result = "此工号已报名,不可重复报名";
                      
                    }
                    else
                    {
                        ExamUserDetailInfo v = new ExamUserDetailInfo();
                        v.UserCode = item.UserCode;
                        v.UserName = item.UserName;
                        v.DepartCode = item.DepartCode;
                        v.PostName = item.PostName;
                        v.PostID = item.PostID;
                        v.RankName = item.RankName;
                        v.SkillName = item.ApplicationLevel; //本职等技能G1
                        v.EntryDate = item.EntryDate; //入职日期
                        v.Achievement = item.Achievement;//绩效
                        v.PracticeScore = item.PracticalID;
                        v.PlanExamDate = item.PlanExamDate;
                        v.ExamStatus = "HrSignUp";
                        v.IsReview = item.IsReview;
                        v.RuleName = item.RuleName;
                        v.TypeName = item.TypeName;
                        v.ApplyLevel = item.ApplicationLevel;//本次申请等级满级                   
                        v.IsAchievement = item.IsAchment;//是否符合绩效
                        v.HighestLevel = item.HighestTestSkill;//最高可考技能
                        v.IsExam = "false";
                        v.IsUserExam = item.IsUserExam;
                        v.HrCreateUser = username;
                        v.HrCreateDate = DateTime.Now;
                        v.SubjectName = item.SubjectName;
                        v.WorkPlace = item.WorkPlace;
                        v.OrgName = item.WorkPlace;
                        v.SignType = item.SignType;
                        v.State = item.State;
                        Data.ExamUserDetailInfo.Insert_ExamUserDetailInfo(v, null, new string[] { "ID" });
                        result = "已报名成功!";
                       
                    }
                   
                };
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        public void GetUserComInfo()
        {
            ListDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrSignUp", IsStop = false });
        }

        public void StopComplete(string ID, string username)
        {
            try
            {
                var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(new { ID = ID });
                c.IsStop = true;
                c.StopCreateDate = DateTime.Now;
                c.StopCreateUser = username;
                Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(c, null, new string[] { "ID" });
                        }
            catch (Exception ex)
            {

                throw;
            }

        }
        public void UploadUserInfo(string filepath)
        {
            DataTable dt = new DataTable();
            FileStream files = null;
            IWorkbook Workbook = null;
            var Ldetailinfo = new List<Entity.ExamUserDetailInfo>();
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
                using (var ds = dt)
                {
                    var q = from DataRow dr in ds.Rows

                            select new Entity.ExamUserDetailInfo
                            {
                                UserCode = dr[0].ToString().Trim(),
                                UserName= dr[1].ToString().Trim(),
                                TypeName = dr[2].ToString().Trim(),
                                SubjectName = dr[3].ToString().Trim()
                            };
                    Ldetailinfo = q.ToList();
                }
                foreach (var item in Ldetailinfo)
                {
                    ExamUserDetailInfo detail = new ExamUserDetailInfo();
                    detail.UserCode = item.UserCode;
                    detail.UserName = item.UserName;
                    detail.TypeName = item.TypeName;
                    detail.SubjectName = item.SubjectName;
                    detail.ExamStatus = "HrSignUp";
                    Data.ExamUserDetailInfo.Insert_ExamUserDetailInfo(detail, null, new string[] { "ID" });
                    Result = "success";
                }

                ListDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrSignUp", IsStop = false });

            }

            catch (Exception ex)
            {

                files.Close();//关闭当前流并释放资源
            }
        }
        public string GetChassisAchieveUser(string startdate, string endate,string username,string sd,string userlist)
        {
            var result = string.Empty;

            SyncQuantityPoint(startdate, endate, userlist, username);
            result = "同步完成";
            return result;
        }
        public string SyncQuantityPoint(string startdate, string endate, string userlist,string username)
        {
            var result = string.Empty;

            MESWebService.ETL_ServiceSoapClient et = new MESWebService.ETL_ServiceSoapClient();
            try
            {
                string sParam = "<root><METHOD ID='Advantech.ETL.ETL.BLL.QryUserQualityPointScore'/>" +
                 "<QUALITY_POINT>" +
                 "<START_DATE >" + startdate + "</START_DATE>" +
                 "<END_DATE>" + endate + "</END_DATE>" +
                 "<USER_LIST>" + userlist + "</USER_LIST>" +
                 "</QUALITY_POINT ></root>";
                var a = et.SFIS_Rv_Xml(sParam, "AKMU3");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(a);
                var YEAR = xmlDoc.GetElementsByTagName("YEAR");
                var MONTH = xmlDoc.GetElementsByTagName("MONTH");
                var USER_NO = xmlDoc.GetElementsByTagName("USER_NO");
                var USER_NAME = xmlDoc.GetElementsByTagName("USER_NAME");
                var POINT_SCORE = xmlDoc.GetElementsByTagName("POINT_SCORE");
                var Listuserl = userlist.Split(new char[] { ',', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var user = USER_NO.Cast<XmlNode>().Select(node => node.InnerText);
                var notlist = Listuserl.Where(x => !user.Any(o=>o==x)).ToList();
                foreach (var item in notlist)
                {
                    var year = string.Format("{0:yyyy}", Convert.ToDateTime(endate).AddMonths(-1));
                    var mom = string.Format("{0:MM}", Convert.ToDateTime(endate).AddMonths(-1));
                    var Scoreinfos = Data.ExamPointScore.Get_All_ExamPointScore(new { UserCode = item, Year = year, Month = mom });
                    if (Scoreinfos.Count()==0)
                    {
                        ExamPointScore score = new ExamPointScore();
                        score.UserCode = item;
                        var name = Data.ExamUserInfo.Get_All_ExamUserInfo(new { UserCode = item });
                        score.UserName = name.FirstOrDefault()?.UserName;
                        score.Year = year;
                        score.Month = mom;
                        score.PointScore = 0;
                        score.CreateUser = username;
                        score.CreateDate = DateTime.Now;
                        Data.ExamPointScore.Insert_ExamPointScore(score, null, new string[] { "ID" });
                    }
                   

                }
                for (int i = 0; i < USER_NO.Count; i++)
                {                   
                    var Scoreinfo = Data.ExamPointScore.Get_All_ExamPointScore(new { UserCode = USER_NO[i].InnerText, Year = YEAR[i].InnerText, Month = MONTH[i].InnerText });
                    if (Scoreinfo.Count()>0&& Scoreinfo!=null)
                    {
                        var existscore = Scoreinfo.FirstOrDefault();
                        existscore.PointScore = Convert.ToInt32(Math.Round(Convert.ToDouble(POINT_SCORE[i].InnerText)));
                        existscore.UpdateUser = username;
                        existscore.UpdateDate = DateTime.Now;
                        Data.ExamPointScore.Update_ExamPointScore(existscore, null, new string[] { "ID" });
                       
                    }
                    else
                    {

                        ExamPointScore score = new ExamPointScore();
                        score.UserCode = USER_NO[i].InnerText;
                        score.UserName = USER_NAME[i].InnerText;
                        score.Year = YEAR[i].InnerText;
                        score.Month = MONTH[i].InnerText;
                        score.PointScore = Convert.ToInt32(Math.Round(Convert.ToDouble(POINT_SCORE[i].InnerText)));
                        score.CreateUser = username;
                        score.CreateDate = DateTime.Now;
                        Data.ExamPointScore.Insert_ExamPointScore(score, null, new string[] { "ID" });

                    }
                    var year = string.Format("{0:yyyy}", Convert.ToDateTime(endate).AddMonths(-1));
                    var mom = string.Format("{0:MM}", Convert.ToDateTime(endate).AddMonths(-1));
                    var Scoreinfos = Data.ExamPointScore.Get_All_ExamPointScore(new { UserCode = USER_NO[i].InnerText, Year = year, Month = mom });
                    if (Scoreinfos.Count() == 0)
                    {
                        ExamPointScore score = new ExamPointScore();
                        score.UserCode = USER_NO[i].InnerText;
                        var name = Data.ExamUserInfo.Get_All_ExamUserInfo(new { UserCode = USER_NO[i].InnerText });
                        score.UserName = name.FirstOrDefault()?.UserName;
                        score.Year = year;
                        score.Month = mom;
                        score.PointScore = 0;
                        score.CreateUser = username;
                        score.CreateDate = DateTime.Now;
                        Data.ExamPointScore.Insert_ExamPointScore(score, null, new string[] { "ID" });
                    }
                    result = "同步完成";
                }
               
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;

        }
    }
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserCode { get; set; }//工号
        public string UserName { get; set; }//姓名
        public string DepartCode { get; set; }//部门代码
        public string PostName { get; set; }//职位
        public string RankName { get; set; }//职等
        public DateTime? EntryDate { get; set; }//入职日
        public string Achievement { get; set; }//绩效
        public string TypeName { get; set; }//考试类型
        public string SubjectName { get; set; }//科目
        public string CreateUser { get; set; }//创建人
        public DateTime? CreateDate { get; set; }//创建时间
        public string UpdateUser { get; set; }//更新人
        public DateTime? UpdateDate { get; set; }//更新时间        
        public string SkillLevel { get; set; }//本职等技能
        public string CurrentSkillLevel { get; set; }//目前技能等级
        public DateTime? LastExamTime { get; set; }// 最近一次考试时间
        public decimal? TheoreticalAchievement { get; set; }//理论成绩
        public string HighestTestSkill { get; set; }//最高可考技能
        public string ApplicationLevel { get; set; }//本次申请等级
        public string IsAchment { get; set; }//是否符合绩效
        public string IsExam { get; set; }//本次是否考试
        public DateTime? PlanExamDate { get; set; }//预计考试时间
        public bool IsApp { get; set; }//是否满级
        public decimal PracticalID { get; set; }//实践主键
        public string ExamStatus { get; set; }
        public bool IsReview { get; set; }
        public string RuleName { get; set; }
        public string ExamScore { get; set; }
        public DateTime? PracticeTime { get; set; }// 最近一次实践考试时间
        public string IsUserExam { get; set; }
        public string ReverseBuckle { get; set; }
        public string ReadExamDate { get; set; }
        public string WorkPlace { get; set; }
        public string ExamineMonth { get; set; }
        public string SignType { get; set; }
        public string PostID { get; set; }
        public string OrgName { get; set; }
        public string State { get; set; }

    }
    public class SearchUserData
    {
        public string TypeName { get; set; }
        public string UserCode { get; set; }
        public string DepartCode { get; set; }
        public string ReadyExamDate { get; set; }
        public string WorkPlace { get; set; }
    }
    public class MesUserInfo
    {
        public string YEAR { get; set; }
        public string MONTH { get; set; }
        public string UserCode { get; set; }
        public string userName { get; set; }
        public string POINT_SCORE { get; set; }
    }
}