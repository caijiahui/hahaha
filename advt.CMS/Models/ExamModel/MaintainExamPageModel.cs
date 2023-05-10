
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using advt.Entity;
using NPOI.SS.Formula.Functions;
using static advt.Entity.Status;

namespace advt.CMS.Models.ExamModel
{
    public class MaintainExamPageModel
    {
        public PageInfo Model { get; set; }
        public List<PageInfo> ListPageInfo { get; set; }
        //public List<ExamUserDetailInfo> ListExamUserDetailInfo { get; set; }
        //public List<ExamType> LExamType { get; set; }
        public List<string> LExamType { get; set; }
        public SerarchData Serarch { get; set; }
        public List<KeyValuePair<string, string>> LWorkPlace { get; set; }
        public MaintainExamPageModel() : base()
        {
            Model = new PageInfo();
            ListPageInfo = new List<PageInfo>();
            //ListExamUserDetailInfo = new List<ExamUserDetailInfo>();
            LExamType = new List<string>();
            Serarch = new SerarchData();
            LExamType = Data.ExamType.Get_All_ExamType().Select(x => x.TypeName).Distinct().ToList();

            LWorkPlace = new List<KeyValuePair<string, string>>();
            LWorkPlace.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo().Where(x => x.OrgName != null).GroupBy(x => x.OrgName))
            {
                LWorkPlace.Add(new KeyValuePair<string, string>(item.Key.ToString(), item.Key.ToString()));
            }
        }
        public void GetPageInfo(SerarchData data)
        {
            var connectionString = "server=172.21.214.28;database=ExamDB;uid=ExamSa;pwd=1Ex@m2021";
            DataSet result = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Proc_Exam_Page_Info", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TypeName", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@UserCode", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@DepartCode", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@SubjectName", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@OrgName", SqlDbType.NVarChar, 50);
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
                    if (data.OrgName != null)
                        cmd.Parameters["@OrgName"].SqlValue = data.OrgName;
                    else
                    {
                        cmd.Parameters["@OrgName"].SqlValue = DBNull.Value;
                    }
                    if (data.TypeName != null)
                        cmd.Parameters["@TypeName"].SqlValue = data.TypeName;
                    else
                    {
                        cmd.Parameters["@TypeName"].SqlValue = DBNull.Value;
                    }
                    if (data.SubjectName != null)
                        cmd.Parameters["@SubjectName"].SqlValue = data.SubjectName;
                    else
                    {
                        cmd.Parameters["@SubjectName"].SqlValue = DBNull.Value;
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(result);
                }
            }
            if (result.Tables.Count != 0)
            {
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    DateTime? oneexam = null;
                    if (row["LExamDate"].ToString() != "")
                    {
                        oneexam = Convert.ToDateTime(row["LExamDate"].ToString());
                    }
                    DateTime? twoexam = null;
                    if (row["ExamDate"].ToString() != "")
                    {
                        twoexam = Convert.ToDateTime(row["ExamDate"].ToString());
                    }
                    DateTime? enterydate = null;
                    if (row["入职日"].ToString() != "")
                    {
                        enterydate = Convert.ToDateTime(row["入职日"].ToString());
                    }
                    ListPageInfo.Add(new PageInfo
                    {
                        UserCode = row["UserCode"].ToString(),
                        UserName = row["UserName"].ToString(),
                        DepartCode = row["DepartCode"].ToString(),
                        RankName = row["RankName"].ToString(),
                        EntryDate = enterydate,
                        TypeNameTwo = row["LTypeName"].ToString(),
                        SubjectNameTwo = row["LSubjectName"].ToString(),
                        ExamDateTwo = oneexam,
                        ExamResultTwo = row["LIsExamPass"].ToString(),
                        PostQuotaTwo = Convert.ToInt32(row["LPostQuota"].ToString()),
                        ElectronicQuotaTwo = Convert.ToInt32(row["LElectronicQuota"].ToString()),
                        SkillsAllowanceTwo = Convert.ToInt32(row["LSkillsAllowance"].ToString()),
                        MajorQuotaTwo = Convert.ToInt32(row["LMajorQuota"].ToString()),
                        TotalQuotaTwo = Convert.ToInt32(row["上次津贴"].ToString()),

                        TypeNameOne = row["TypeName2"].ToString(),
                        SubjectNameOne = row["SubjectName"].ToString(),
                        ExamDateOne = twoexam,
                        ExamResultOne = row["考试结果"].ToString(),
                        PostQuotaOne = Convert.ToInt32(row["岗位等级"].ToString()),
                        ElectronicQuotaOne = Convert.ToInt32(row["岗位津贴"].ToString()),
                        SkillsAllowanceOne = Convert.ToInt32(row["技能津贴"].ToString()),
                        MajorQuotaOne = Convert.ToInt32(row["专业加给"].ToString()),
                        TotalQuotaOne = Convert.ToInt32(row["累计津贴"].ToString()),
                        AddData = Convert.ToInt32(row["总金额"].ToString()),
                        TakeEffDate = twoexam
                    });
                }
            }



            #region
            //var ListExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserALLDetailInfo(data.UserCode, data.SubjectName, data.TypeName, data.OrgName, data.DepartCode).OrderByDescending(x => x.ExamDate != null&&!x.IsStop&& x.IsExam == "true"&&x.ExamStatus == "HrCheck").Where(x => x.State == "试用" || x.State == "正式");
            //var user = ListExamUserDetailInfo.Select(x => x.UserCode).Distinct();


            //foreach (var item in user)
            //{

            //    var ss = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = item, IsStop = false, IsExam = "true", ExamStatus="HrCheck" }).OrderByDescending(x => x.ExamDate).Take(2);
            //    var seclst = ss?.Take(1).FirstOrDefault();               
            //    var seclsts = ss.Count()>1? ss.Take(2).LastOrDefault():null;
            //    var userinfo = Data.ExamUserInfo.Get_All_ExamUserInfo(new { UserCode = item })?.FirstOrDefault();

            //    int TotalQuotaTwo = 0;
            //    var ExamResultOne = string.Empty;
            //    int PostQuotaOne = 0;
            //    int ElectronicQuotaOne = 0;
            //    int SkillsAllowanceOne = 0;
            //    int MajorQuotaOne = 0;
            //    int TotalQuotaOne = 0;
            //    int AddQua = 0;
            //    string TypeNameTwo = string.Empty;
            //    string SubjectNameTwo = string.Empty;
            //    DateTime? ExamDateTwo = null;
            //    string ExamResultTwo = string.Empty;
            //    int PostQuotaTwo = 0;
            //    int ElectronicQuotaTwo = 0;
            //    int SkillsAllowanceTwo = 0;
            //    int MajorQuotaTwo = 0;
            //    var sub = Data.ExamSubject.Get_All_ExamSubject(new { SubjectName = seclst.SubjectName })?.FirstOrDefault();
            //    ExamResultOne = seclst.IsExamPass ? "通过" : "未通过";
            //    if (seclsts != null)
            //    {
            //        TypeNameTwo = seclsts?.Type == "取消" ? "取消" : seclsts.TypeName;
            //        SubjectNameTwo = seclsts?.SubjectName;
            //        ExamDateTwo = seclsts?.ExamDate;
            //        ExamResultTwo = seclsts.IsExamPass == true ? "通过" : "未通过";
            //        PostQuotaTwo = seclsts.PostQuota;
            //        ElectronicQuotaTwo = seclsts.ElectronicQuota;
            //        SkillsAllowanceTwo = seclsts.SkillsAllowance;
            //        MajorQuotaTwo = seclsts.MajorQuota;
            //        TotalQuotaTwo = seclsts.TotalQuota;
            //    }
            //    if (sub != null)
            //    {
            //        if (ExamResultOne == "通过")
            //        {
            //            PostQuotaOne = sub.PostQuota;
            //            ElectronicQuotaOne = sub.ElectronicQuota;
            //            SkillsAllowanceOne = sub.SkillsAllowance;
            //            MajorQuotaOne = sub.MajorQuota;
            //        }
            //        else if (ExamResultOne == "未通过")
            //        {
            //            PostQuotaOne = seclst.PostQuota;
            //            ElectronicQuotaOne = seclst.ElectronicQuota;
            //            SkillsAllowanceOne = seclst.SkillsAllowance;
            //            MajorQuotaOne = seclst.MajorQuota;
            //        }

            //        TotalQuotaOne = PostQuotaOne + ElectronicQuotaOne + SkillsAllowanceOne + MajorQuotaOne;
            //    }

            //    if (!sub.IsAddAllowance)
            //    {
            //        var rank = Data.RankInfo.Get_All_RankInfo(new { RankName = seclst.RankName })?.FirstOrDefault();
            //        if (seclst.TypeName == "关键岗位技能等级" && rank?.SkillName == seclst.ApplyLevel)
            //            AddQua = 0;
            //        else
            //            AddQua = TotalQuotaOne;
            //    }
            //    else
            //    {
            //        AddQua = TotalQuotaOne - TotalQuotaTwo;
            //    }
            //    ListPageInfo.Add(new PageInfo
            //    {
            //        UserCode = userinfo.UserCode,
            //        UserName = userinfo.UserName,
            //        DepartCode = userinfo.DepartCode,
            //        RankName = userinfo.RankName,
            //        EntryDate = userinfo.EntryDate,

            //        TypeNameTwo = TypeNameTwo,
            //        SubjectNameTwo = SubjectNameTwo,
            //        ExamDateTwo = ExamDateTwo,
            //        ExamResultTwo = ExamResultTwo,
            //        PostQuotaTwo = PostQuotaTwo,
            //        ElectronicQuotaTwo = ElectronicQuotaTwo,
            //        SkillsAllowanceTwo = SkillsAllowanceTwo,
            //        MajorQuotaTwo = MajorQuotaTwo,
            //        TotalQuotaTwo = TotalQuotaTwo,

            //        TypeNameOne = seclst.Type != "取消" ? seclst.TypeName : "取消",
            //        SubjectNameOne = seclst?.SubjectName,
            //        ExamDateOne = seclst?.ExamDate,
            //        ExamResultOne = ExamResultOne,
            //        PostQuotaOne = PostQuotaOne,
            //        ElectronicQuotaOne = ElectronicQuotaOne,
            //        SkillsAllowanceOne = SkillsAllowanceOne,
            //        MajorQuotaOne = MajorQuotaOne,
            //        TotalQuotaOne = TotalQuotaOne, 
            //        AddData= AddQua,
            //        TakeEffDate = seclst?.ExamDate
            //    });
            //}
            #endregion

        }

        public MemoryStream GetPageExcelInfo(string UserCode, string SubjectName, string TypeName, string OrgName, string DepartCode)
        {
            try
            {
                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                var model = new ExamUserDetailInfo();
                var c = Data.ExamUserDetailInfo.Get_All_ExamUserALLDetailInfo(UserCode,  SubjectName,  TypeName,  OrgName,  DepartCode);
                //获取list数据
                var tlst = c;
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("工号");
                row1.CreateCell(1).SetCellValue("姓名");
                row1.CreateCell(2).SetCellValue("部门");
                row1.CreateCell(3).SetCellValue("科目");
                row1.CreateCell(4).SetCellValue("理论成绩");
                row1.CreateCell(5).SetCellValue("职称");
                row1.CreateCell(6).SetCellValue("职等");
                row1.CreateCell(7).SetCellValue("入职日期");
                row1.CreateCell(8).SetCellValue("本职等技能");
                row1.CreateCell(9).SetCellValue("目前技能等级");
                row1.CreateCell(10).SetCellValue("考试时间");               
                
                for (int i = 0; i < tlst.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(tlst[i].UserCode);//工号
                    rowtemp.CreateCell(1).SetCellValue(tlst[i].UserName);//姓名
                    rowtemp.CreateCell(2).SetCellValue(tlst[i].DepartCode);//部门
                    rowtemp.CreateCell(3).SetCellValue(tlst[i].SubjectName);//科目
                    rowtemp.CreateCell(4).SetCellValue(tlst[i].ExamScore.ToString());//理论成绩
                    rowtemp.CreateCell(5).SetCellValue(tlst[i].PostName);//职称
                    rowtemp.CreateCell(6).SetCellValue(tlst[i].RankName);//职等
                    rowtemp.CreateCell(7).SetCellValue(tlst[i].EntryDate.ToString());//入职日期
                    rowtemp.CreateCell(8).SetCellValue(tlst[i].SkillName);//本职等技能
                    rowtemp.CreateCell(9).SetCellValue(tlst[i].ApplyLevel);//目前技能等级
                    rowtemp.CreateCell(10).SetCellValue(tlst[i].UserExamDate.ToString());//考试时间 
                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;


            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class PageInfo
    {
        public string UserCode { get; set; }//工号
        public string UserName { get; set; }//姓名
        public string DepartCode { get; set; }//部门
        public string RankName { get; set; }//职等
        public DateTime? EntryDate { get; set; }//入职日期
        //考试类型 考试科目    考试日期 考试结果    岗位等级 岗位津贴    技能津贴 专业加给    津贴总额
        public string TypeNameOne { get; set; }
        public string SubjectNameOne { get; set; }
        public DateTime? ExamDateOne { get; set; }
        public string ExamResultOne { get; set; }
        public int PostQuotaOne { get; set; }//岗位等级
        public int ElectronicQuotaOne { get; set; }//岗位津贴
        public int SkillsAllowanceOne { get; set; }//技能津贴       
        public int MajorQuotaOne { get; set; }//专业加给    
        public int TotalQuotaOne { get; set; }//汇总津贴
     
        //考试类型 考试科目    考试日期 考试结果    岗位等级 岗位津贴    技能津贴 专业加给    津贴总额
        public string TypeNameTwo { get; set; }
        public string SubjectNameTwo { get; set; }
        public DateTime? ExamDateTwo { get; set; }
        public string ExamResultTwo{ get; set; }
        public int PostQuotaTwo { get; set; }//岗位等级
        public int ElectronicQuotaTwo { get; set; }//岗位津贴
        public int SkillsAllowanceTwo { get; set; }//技能津贴       
        public int MajorQuotaTwo { get; set; }//专业加给    
        public int TotalQuotaTwo { get; set; }//汇总津贴
        //本次考核后增加总额
        public int AddData { get; set; }
        public DateTime? TakeEffDate { get; set; }//生效日期


    }
    public class SerarchData 
    { 
     public string TypeName { get; set; }
     public string SubjectName { get; set; }
     public string OrgName { get; set; }
     public string DepartCode { get; set; }
     public string UserCode { get; set; }
     public string IsExam { get; set; }
    }
}