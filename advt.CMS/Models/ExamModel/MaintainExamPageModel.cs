
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
        public List<ExamUserDetailInfo> ListExamUserDetailInfo { get; set; }
        public MaintainExamPageModel() : base()
        {
            Model = new PageInfo();
            ListPageInfo = new List<PageInfo>();
            ListExamUserDetailInfo = new List<ExamUserDetailInfo>();
        }
        public void GetPageInfo(string UserCode,string SubjectName,string ExamDate,string DepartCode)
        {
            ListExamUserDetailInfo = Data.ExamUserDetailInfo.Get_All_ExamUserGetDetailInfo(UserCode,SubjectName, ExamDate, DepartCode);
            if (ListExamUserDetailInfo.Count() > 0 && ListExamUserDetailInfo != null)
            {
                foreach (var item in ListExamUserDetailInfo)
                {
                    //实践成绩

                    ////申请等级
                    //var SkillName = "";
                    ////岗位津贴
                    //var Allowance = Data.SkillInfo.Get_All_SkillInfo(new { SkillName = SkillName });

                    ////技能等级通过后可晋升的新职等
                    //var NewRankName = "";

                    ////晋升加给
                    //var PromotionBonus = "";
                    //var Bonus = Data.RankInfo.get(new { SkillName = SkillName });
                    DateTime? prdate = null;
                    if (!string.IsNullOrEmpty(item.SkillName))
                    {
                        var pralist = Data.PracticeInfo.Get_All_PracticeInfo(new { SkillName =item.ApplyLevel, UserCode =item.UserCode});
                        if (pralist.Count() > 0 && pralist != null)
                        {
                            prdate = pralist.OrderByDescending(x => x.CreateDate).FirstOrDefault().CreateDate;
                        }
                    }
                    DateTime? entrydate = null;
                    var PostName = "";
                    var rank = "";
                    var level = "";
                    if (!string.IsNullOrEmpty(item.UserCode))
                    {
                        var userinfo = Data.ExamUserInfo.Get_All_ExamUserInfo(new { UserCode=item.UserCode});
                        if (userinfo.Count() > 0 && userinfo != null)
                        {
                            entrydate = userinfo.FirstOrDefault().EntryDate;
                            PostName = userinfo.FirstOrDefault().PostName;
                            rank = userinfo.FirstOrDefault().RankName;
                            level = userinfo.FirstOrDefault().ApplicationLevel;
                        }
                    }

                        ListPageInfo.Add(new PageInfo
                    {
                        UserCode = item.UserCode,
                        UserName = item.UserName,
                        DepartCode = item.DepartCode,
                        PostName = PostName,
                        RankName =rank,
                        EntryDate = entrydate,
                        OrganizingFunction = "",
                        CurrentLevel = item.SkillName,//本职等技能
                        ApplyLevel = level,//目前技能等级
                        CurrectExamDate = item.UserExamDate,//最近一次考试时间
                        SubjectName = item.SubjectName,
                        ExamScore = item.ExamScore,//最近一次理论成绩
                        PracticeScore = item.PracticeScore,
                        PracticeDate = prdate,//最近一次实践成绩通过时间
                        FullScale = "是"
                    });
                }
            }


        }

        public MemoryStream GetPageExcelInfo(string UserCode, string SubjectName, string DepartCode, string ExamDate)
        {
            try
            {


                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                var model = new ExamUserDetailInfo();
                var c = Data.ExamUserDetailInfo.Get_All_ExamUserGetDetailInfo(UserCode, SubjectName, ExamDate, DepartCode);
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
        public string PostName { get; set; }//职称
        public string RankName { get; set; }//职等
        public DateTime? EntryDate { get; set; }//入职日期
        public string PostState { get; set; }//在职状态
        public string PostJob { get; set; }//本职岗位
        public string SubjectName { get; set; }//考核岗位
        public string SkillName { get; set; } //申请等级
        public decimal? ExamScore { get; set; }//理论考核
        public decimal? PracticeScore { get; set; }//实践考核
        public int SkillAllowance { get; set; }//标准等级津贴
        public string NewRankName { get; set; }//技能等级通过后可晋升的新职等
        public int PromotionBonus { get; set; }//晋升加给
        public int ToatlBonus { get; set; } //此次上调金额
        public string  CurrentMonth { get; set; }//薪资调整月份
        public string CurrentLevel { get; set; }  //本职等技能
        public string ApplyLevel { get; set; }  //目前技能等级
        public DateTime? CurrectExamDate { get; set; }  //最近一次考试时间
        public DateTime? PracticeDate { get; set; }  //最近一次实践成绩通过时间
        public string FullScale { get; set; }//是否满级
        public string OrganizingFunction { get; set; }//组织职能性质
    }
}