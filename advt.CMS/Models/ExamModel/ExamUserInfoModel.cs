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
    public class ExamUserInfoModel
    {
        public UserInfo UserInfoList { get; set; }
        public List<UserInfo> ListUserInfo { get; set; }
        public List<Entity.RankInfo> ListRankInfo { get; set; }
        public List<Entity.RankInfo> ListRank { get; set; }
        
        public List<Entity.ExamUserDetailInfo> ListExamUserDetailInfo { get; set; }
        public string Result { get; set; }
        public List<Entity.PracticeInfo> ListPracticeInfo { get; set; }
        public Entity.ExamUserInfo VExamUserInfo { get; set; }
        public Entity.ExamUserDetailInfo VExamUserDetailInfo { get; set; }
        public List<Entity.ExamUserInfo> ListExamUserInfo { get; set; }
        public List<Entity.SkillInfo> ListSkillInfo { get; set; }
        public ExamUserInfoModel() : base()
        {
            UserInfoList = new UserInfo();
            ListUserInfo = new List<UserInfo>();
            ListRankInfo = new List<Entity.RankInfo>();
            ListExamUserDetailInfo = new List<Entity.ExamUserDetailInfo>();
            ListPracticeInfo = new List<Entity.PracticeInfo>();
            VExamUserInfo = new Entity.ExamUserInfo();
            ListExamUserInfo = new List<Entity.ExamUserInfo>();
            VExamUserDetailInfo = new Entity.ExamUserDetailInfo();
            ListRank = new List<Entity.RankInfo>();
            ListSkillInfo = new List<Entity.SkillInfo>();

        }
        public void GetUserInfo()
        {
            var user = Data.ExamUserInfo.Get_All_ExamUserInfo();
            foreach (var item in user)
            {
                //本职等技能,最高渴考技能
                var SkillLevel = "";
                var maxskilllevel = "";
                if (item.RankName != null)
                {
                    ListRankInfo = Data.ExamUserInfo.Get_ExamUserLevel(item.RankName);
                    if (ListRankInfo != null&& ListRankInfo.Count()>0)
                    {
                        SkillLevel = ListRankInfo.FirstOrDefault().SkillName;
                        maxskilllevel = ListRankInfo.FirstOrDefault().MaxSkillName;
                    }
                }
                //目前技能等级,最近一次考试时间
                var CurrentSkillLevel = "";
                DateTime LastExamTime = DateTime.Now;
                DateTime PlanTime = DateTime.Now;
                decimal? score = 0;
                if (item.UserCode != null)
                {
                    ListExamUserDetailInfo = Data.ExamUserInfo.GetExamUserDetail(item.UserCode);
                    if (ListExamUserDetailInfo != null&& ListExamUserDetailInfo.Count()>0)
                    {
                        //目前技能
                        CurrentSkillLevel = ListExamUserDetailInfo.FirstOrDefault().SkillName;
                        LastExamTime =Convert.ToDateTime(ListExamUserDetailInfo.FirstOrDefault().ExamDate);
                        PlanTime = Convert.ToDateTime(ListExamUserDetailInfo.FirstOrDefault().PlanExamDate);
                        if (CurrentSkillLevel != null && LastExamTime != null)
                        {
                            //理论成绩（实践表）
                            ListPracticeInfo = Data.ExamUserInfo.GetPraticeScore(item.UserCode, CurrentSkillLevel);
                            if (ListPracticeInfo != null&& ListPracticeInfo.Count()>0)
                            {
                                score = ListPracticeInfo.FirstOrDefault().PracticeScore;
                            }
                        }
                    }

                }
                //ApplicationLevel 本地申请等级
                var ApplicationLevel = "";
                bool apps = false;
                if (item.UserCode != null)
                {
                    ListExamUserDetailInfo = Data.ExamUserInfo.GetExamUserDetail(item.UserCode);
                    if (ListExamUserDetailInfo.Count() > 0)
                    {
                        var ap= "";
                        var app = ListExamUserDetailInfo.FirstOrDefault().ApplyLevel;
                        if (app != null)
                        {
                            if (app == "G1")
                            {
                                ap = "G2";
                            }
                            else if (app == "G2")
                            {
                                ap = "G3";
                            }
                            else if (app == "G3")
                            {
                                ap = "G4";
                            }
                            else if (app == "G4")
                            {
                                ap = "G5";
                            }
                            else if (app == "G5")
                            {
                                ap = "G6";
                            }
                            else if (app == "G6")
                            {
                                ap = "G7";
                            }
                            else if (app == "G7")
                            {
                                ap = "G8";
                            }
                            //本职等技能 + 1，如果去职等表里面查找不到，就显现满级，如果存在，就显示对应的职等技能
                              ListRankInfo = Data.ExamUserInfo.GetRanKInfoSkill(ap);
                            if (ListRankInfo != null&& ListRankInfo.Count()>0)
                            {
                                var s = ListRankInfo.FirstOrDefault().SkillName;
                                ApplicationLevel = s;
                               
                            }
                            else {
                                ApplicationLevel = app;
                            }

                        }
                    }
                    else
                    {
                        //不存在，就按照职等去技能表找对应的本职等级能
                        ListRankInfo = Data.ExamUserInfo.Get_ExamUserLevel(item.RankName);
                        if (ListRankInfo != null)
                        {
                            ApplicationLevel = ListRankInfo.FirstOrDefault().SkillName;
                            apps = true;
                        }

                    }
                }
                //是否绩效
                bool isach = false;
                if (ApplicationLevel != null)
                {
                    ListSkillInfo = Data.ExamUserInfo.GetSkillAch(ApplicationLevel);
                    if (ListSkillInfo != null&&ListSkillInfo.Count()>0)
                    {
                        var sk = ListSkillInfo.FirstOrDefault().AchRequire;
                        if (sk == item.Achievement)
                        {
                            isach = true;
                        }
                       
                    }
                }

                //本次是否考试
                bool isexam = false;
                if (apps == false && isach == true)
                {
                    isexam = true;
                }

                ListUserInfo.Add(new UserInfo {
                    Id = item.ID,
                    UserCode = item.UserCode,
                    UserName = item.UserName,
                    DepartCode = item.DepartCode,
                    PostName = item.PostName,
                    RankName = item.RankName,
                    EntryDate = item.EntryDate,
                    Achievement = item.Achievement,
                    TypeName = item.TypeName,
                    SubjectName = item.SubjectName,
                    CreateUser = item.CreateUser,
                    CreateDate = item.CreateDate,
                    UpdateUser = item.UpdateUser,
                    UpdateDate = item.UpdateDate,
                    SkillLevel = SkillLevel,
                    HighestTestSkill=maxskilllevel,
                    CurrentSkillLevel= CurrentSkillLevel,
                    LastExamTime= LastExamTime,
                    TheoreticalAchievement=score,
                    IsApp=apps,
                    IsAchment= isach,
                    PlanExamDate=PlanTime,
                    IsExam=isexam,
                    ApplicationLevel= ApplicationLevel
                });
            }
        }

        public void UploadUser(string filepath)
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
                    Data.ExamUserInfo.Get_UpdateExamUserInfo(item.UserCode,item.Achievement);
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
        public bool IsAchment { get; set; }//是否符合绩效
        public bool IsExam { get; set; }//本次是否考试
        public DateTime? PlanExamDate { get; set; }//预计考试时间
        public bool IsApp { get; set; }//是否满级

    }
}