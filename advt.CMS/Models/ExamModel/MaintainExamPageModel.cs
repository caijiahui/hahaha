
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using advt.Entity;

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
        }
        public void GetPageInfo(SerarchData data)
        {
            LExamType = Data.ExamType.Get_All_ExamType().Select(x => x.TypeName).Distinct().ToList();
              
            LWorkPlace = new List<KeyValuePair<string, string>>();
            LWorkPlace.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo().Where(x => x.OrgName != null).GroupBy(x => x.OrgName))
            {
                LWorkPlace.Add(new KeyValuePair<string, string>(item.Key.ToString(), item.Key.ToString()));
            }
            var ListExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserALLDetailInfo(data.UserCode, data.SubjectName, data.TypeName, data.OrgName, data.DepartCode);
            foreach (var item in ListExamUserDetailInfo.OrderByDescending(x=>x.ExamDate).Where(x=>x.State== "试用" || x.State == "正式"))
            {
                var seclst = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { UserCode = item.UserCode, IsStop = false, IsExam = "true" }).OrderByDescending(x => x.ExamDate).Take(2);
                
                var TypeNameTwo = string.Empty;
                var SubjectNameTwo = string.Empty;
                DateTime? ExamDateTwo =null;
                var ExamResultTwo= string.Empty;
                int PostQuotaTwo = 0;
                int ElectronicQuotaTwo = 0;
                int SkillsAllowanceTwo = 0;
                int MajorQuotaTwo = 0;
                int TotalQuotaTwo = 0;
                var TypeNameOne = string.Empty;
                var SubjectNameOne = string.Empty;
                DateTime? ExamDateOne = null;
                var ExamResultOne = string.Empty;
                int PostQuotaOne = 0;
                int ElectronicQuotaOne = 0;
                int SkillsAllowanceOne = 0;
                int MajorQuotaOne = 0;
                int TotalQuotaOne = 0;

                var sub = Data.ExamSubject.Get_All_ExamSubject(new { SubjectName = seclst.FirstOrDefault().SubjectName });
                //本次
                TypeNameOne = seclst.FirstOrDefault().TypeName;
                SubjectNameOne = seclst.FirstOrDefault().SubjectName;
                ExamDateOne = seclst.FirstOrDefault().ExamDate;
                ExamResultOne = seclst.FirstOrDefault().IsExamPass ? "通过" : "未通过";
                if (sub.Count() > 0)
                {
                    if (ExamResultOne == "通过")
                    {
                        PostQuotaOne = sub.FirstOrDefault().PostQuota;
                        ElectronicQuotaOne = sub.FirstOrDefault().ElectronicQuota;
                        SkillsAllowanceOne = sub.FirstOrDefault().SkillsAllowance;
                        MajorQuotaOne = sub.FirstOrDefault().MajorQuota;
                    }
                    else
                    {
                        PostQuotaOne = seclst.FirstOrDefault().PostQuota;
                        ElectronicQuotaOne = seclst.FirstOrDefault().ElectronicQuota;
                        SkillsAllowanceOne = seclst.FirstOrDefault().SkillsAllowance;
                        MajorQuotaOne = seclst.FirstOrDefault().MajorQuota;
                    }
                    TotalQuotaOne = PostQuotaOne + ElectronicQuotaOne + SkillsAllowanceOne + MajorQuotaOne;
                }
                if (seclst.Count()==2)
                {
                    //上次
                    TypeNameTwo = seclst.LastOrDefault().TypeName;
                    SubjectNameTwo = seclst.LastOrDefault().SubjectName;
                    ExamDateTwo = seclst.LastOrDefault().ExamDate;
                    ExamResultTwo = seclst.LastOrDefault().IsExamPass ? "通过" : "未通过";
                    PostQuotaTwo = seclst.LastOrDefault().PostQuota;
                    ElectronicQuotaTwo = seclst.LastOrDefault().ElectronicQuota;
                    SkillsAllowanceTwo = seclst.LastOrDefault().SkillsAllowance;
                    MajorQuotaTwo = seclst.LastOrDefault().MajorQuota;
                    TotalQuotaTwo = seclst.LastOrDefault().TotalQuota;
                }
                ListPageInfo.Add(new PageInfo
                {
                    UserCode = item.UserCode,
                    UserName = item.UserName,
                    DepartCode = item.DepartCode,
                    RankName = item.RankName,
                    EntryDate = item.EntryDate,

                    TypeNameTwo = TypeNameTwo,
                    SubjectNameTwo = SubjectNameTwo,
                    ExamDateTwo = ExamDateTwo,
                    ExamResultTwo = ExamResultTwo,
                    PostQuotaTwo = PostQuotaTwo,
                    ElectronicQuotaTwo = ElectronicQuotaTwo,
                    SkillsAllowanceTwo = SkillsAllowanceTwo,
                    MajorQuotaTwo = MajorQuotaTwo,
                    TotalQuotaTwo = TotalQuotaTwo,

                    TypeNameOne = TypeNameOne,
                    SubjectNameOne = SubjectNameOne,
                    ExamDateOne = ExamDateOne,
                    ExamResultOne = ExamResultOne,
                    PostQuotaOne = PostQuotaOne,
                    ElectronicQuotaOne = ElectronicQuotaOne,
                    SkillsAllowanceOne = SkillsAllowanceOne,
                    MajorQuotaOne = MajorQuotaOne,
                    TotalQuotaOne = TotalQuotaOne,

                   

                    AddData= TotalQuotaOne - TotalQuotaTwo,
                    TakeEffDate = ExamDateOne
                });
            }

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