using advt.Entity;
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
        public string Results { get; set; }
        public bool IsHrSign { get; set; }
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

            LExamType.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamType.Get_All_ExamType())
            {
                LExamType.Add(new KeyValuePair<string, string>(item.ID.ToString(), item.TypeName));
            }
        }
        public void GetUserInfo(SearchUserData data)
        {
            var connectionString = "server=172.21.161.41;database=ExamDB;uid=ExamSa;pwd=1Ex@m2021";
            DataSet result = new DataSet();
            data.TypeName = "53";
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
                            ApplicationLevel = row["ApplicationLevel"].ToString(),//本次申请等级      
                            IsAchment = row["IsAchment"].ToString(),
                            Achievement = row["Achievement"].ToString(),
                            SubjectName = row["SubjectName"].ToString(),
                            ReverseBuckle = row["ReverseBuckle"].ToString(),//最初职等
                            IsUserExam = row["IsUserExam"].ToString()
                        });
                    }
                    ListUserInfo11 = ListUserInfo.ToList();
                    YListUserInfo = ListUserInfo.Where(x => x.IsUserExam == "true").ToList();
                    ListDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ExamStatus = "HrSignUp", IsStop = false, TypeName = data.TypeName });
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
                    Data.AchieveRecord.Insert_AchieveRecord(record);

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
                    listuserdatail = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode=item.UserCode,SubjectName=item.SubjectName,IsStop=false });
                    if (listuserdatail != null && listuserdatail.Count() > 0)
                    {
                        result += "此工号已报名,不可重复报名";
                      
                    }
                    else
                    {
                        ExamUserDetailInfo v = new ExamUserDetailInfo();
                        v.UserCode = item.UserCode;
                        v.UserName = item.UserName;
                        v.DepartCode = item.DepartCode;
                        v.PostName = item.PostName;
                        v.RankName = item.RankName;
                        v.SkillName = item.SkillLevel; //本职等技能G1
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
                        v.IsExam ="false";
                        v.IsUserExam = item.IsUserExam;
                        v.HrCreateUser = username;
                        v.HrCreateDate = DateTime.Now;
                        v.SubjectName = item.SubjectName;
                        Data.ExamUserDetailInfo.Insert_ExamUserDetailInfo(v, null, new string[] { "ID" });
                        result += "已报名成功!";
                       
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
                c.HrCheckCreateDate = DateTime.Now;
                c.HrCheckCreateUser = username;
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

    }
    public class SearchUserData
    {
        public string TypeName { get; set; }
        public string UserCode { get; set; }
        public string DepartCode { get; set; }
    }
}