using advt.Entity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class ExamBankModel
    {
        public List<ExamBank> LExamBank { get; set; }
        public ExamBank VExamBank { get; set; }
        //public List<ExamType> LExamType { get; set; }
        public List<KeyValuePair<string, string>> LExamType { get; set; }
        public List<KeyValuePair<string, string>> LExamSubject { get; set; }
        public List<KeyValuePair<string, string>> LTopicType { get; set; }
        public string Result { get; set; }
        public List<ExamSubject> ListExamSubject { get; set; }
        public List<ExamBank> ListTopicLevel { get; set; }
        public string BankRemark { get; set; }
        
        public ExamBankModel() : base()
        {
            LExamSubject = new List<KeyValuePair<string, string>>();
            LExamBank = new List<ExamBank>();
            LExamType = new List<KeyValuePair<string, string>>();
            VExamBank = new ExamBank();
            LTopicType = new List<KeyValuePair<string, string>>();
            LTopicType.Add(new KeyValuePair<string, string>("L1", "单选"));
            LTopicType.Add(new KeyValuePair<string, string>("L2", "多选"));
            LTopicType.Add(new KeyValuePair<string, string>("L3", "问答"));
            ListExamSubject = new List<ExamSubject>();
            ListTopicLevel = new List<ExamBank>();
        }
        public void UploadBank(string filepath)
        {
            DataTable dt = new DataTable();
            FileStream files = null;
            IWorkbook Workbook = null;
            var LBank = new List<ExamBank>();
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
                                        dataRow[j] = row.GetCell(j).ToString();
                                }
                            }

                            dt.Rows.Add(dataRow);
                        }
                    }
                }
                using (var ds = dt)
                {
                    var q = from DataRow dr in ds.Rows

                            select new Entity.ExamBank
                            {
                                ExamType = dr[1].ToString().Trim(),
                                ExamSubject = dr[2].ToString().Trim(),
                                //ExamSubject =!string.IsNullOrEmpty(dr[2].ToString().Trim())!=true? new Regex("(?<=;) +").Replace(dr[2].ToString().Trim(), ""):null,
                                TopicMajor = dr[3].ToString().Trim(),
                                TopicLevel = dr[4].ToString().Trim()==null?"": dr[4].ToString().Trim(),
                                TopicType = dr[5].ToString().Trim(),
                                TopicTitle = dr[6].ToString().Trim(),
                                TopicTitlePicNum = dr[7].ToString().Trim(),
                                RightKey = dr[8].ToString().Trim(),
                                Remark = dr[9].ToString().Trim(),
                                OptionA = dr[10].ToString().Trim(),
                                OptionAPicNum = string.IsNullOrEmpty(dr[11].ToString().Trim()) == true ? null : "~/Attachment/BankPic" + dr[10].ToString().Trim(),
                                OptionB = dr[12].ToString().Trim(),
                                OptionBPicNum = string.IsNullOrEmpty(dr[13].ToString().Trim()) == true ? null : "~/Attachment/BankPic" + dr[12].ToString().Trim(),
                                OptionC = dr[14].ToString().Trim(),
                                OptionCPicNum = string.IsNullOrEmpty(dr[15].ToString().Trim()) == true ? null : "~/Attachment/BankPic" + dr[14].ToString().Trim(),
                                OptionD = dr[16].ToString().Trim(),
                                OptionDPicNum = dr[17].ToString().Trim(),
                                OptionE = dr[18].ToString().Trim(),
                                OptionEPicNum = string.IsNullOrEmpty(dr[19].ToString().Trim()) == true ? null : "~/Attachment/BankPic" + dr[18].ToString().Trim(),
                                OptionF = dr[20].ToString().Trim(),
                                OptionFPicNum = string.IsNullOrEmpty(dr[21].ToString().Trim()) == true ? null : "~/Attachment/BankPic" + dr[20].ToString().Trim(),
                                CreateDate = DateTime.Now
                            };
                    LBank = q.ToList();
                }
                var successcount = 0;
                foreach (var item in LBank)
                {
                    var c = Data.ExamBank.Insert_ExamBank(item, null, new string[] { "ID" });
                    successcount += c;
                }
                Result = successcount + "成功插入" + successcount + "记录";
                LExamBank = Data.ExamBank.Get_All_ExamBank();

            }

            catch (Exception ex)
            {
                Result = ex.Message;
                files.Close();//关闭当前流并释放资源
            }
        }

        public MemoryStream SignUpBank(string ExamType, string ExamSubject, string ExamMajor, string ExamLevel, string ExamContent)
        {
            try
            {


                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
                var model = new ExamBankModel();
                var c = Data.ExamBank.Get_All_ExamBank_ExamType_Subject(ExamType, ExamSubject, ExamMajor, ExamLevel, ExamContent);
                //获取list数据
                var tlst = c;
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("考试类型");
                row1.CreateCell(1).SetCellValue("考试科目");
                row1.CreateCell(2).SetCellValue("题目专业");
                row1.CreateCell(3).SetCellValue("题目等级");
                row1.CreateCell(4).SetCellValue("题目类型");
                row1.CreateCell(5).SetCellValue("题目内容");
                row1.CreateCell(6).SetCellValue("题目内容图片");
                row1.CreateCell(7).SetCellValue("正确答案");
                row1.CreateCell(8).SetCellValue("答案解析");
                row1.CreateCell(9).SetCellValue("选项A");
                row1.CreateCell(10).SetCellValue("选项A图片");
                row1.CreateCell(11).SetCellValue("选项B");
                row1.CreateCell(12).SetCellValue("选项B图片");
                row1.CreateCell(13).SetCellValue("选项C");
                row1.CreateCell(14).SetCellValue("选项C图片");
                row1.CreateCell(15).SetCellValue("选项D");
                row1.CreateCell(16).SetCellValue("选项D图片");
                row1.CreateCell(17).SetCellValue("选项E");
                row1.CreateCell(18).SetCellValue("选项E图片");
                row1.CreateCell(19).SetCellValue("选项F");
                row1.CreateCell(20).SetCellValue("选项F图片");

                //将数据逐步写入sheet1各个行
                for (int i = 0; i < tlst.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(tlst[i].ExamType);//考试类型
                    rowtemp.CreateCell(1).SetCellValue(tlst[i].ExamSubject);//考试科目
                    rowtemp.CreateCell(2).SetCellValue(tlst[i].TopicMajor);//题目专业
                    rowtemp.CreateCell(3).SetCellValue(tlst[i].TopicLevel);//题目等级
                    rowtemp.CreateCell(4).SetCellValue(tlst[i].TopicType);//题目类型
                    rowtemp.CreateCell(5).SetCellValue(tlst[i].TopicTitle);//题目内容
                    rowtemp.CreateCell(6).SetCellValue(tlst[i].TopicTitlePicNum);//题目内容图片
                    rowtemp.CreateCell(7).SetCellValue(tlst[i].RightKey);//正确答案
                    rowtemp.CreateCell(8).SetCellValue(tlst[i].Remark);//答案解析
                    rowtemp.CreateCell(9).SetCellValue(tlst[i].OptionA);//选项A
                    rowtemp.CreateCell(10).SetCellValue(tlst[i].OptionAPicNum);//选项A图片
                    rowtemp.CreateCell(11).SetCellValue(tlst[i].OptionB);//选项B
                    rowtemp.CreateCell(12).SetCellValue(tlst[i].OptionBPicNum);//选项B图片
                    rowtemp.CreateCell(13).SetCellValue(tlst[i].OptionC);//选项C
                    rowtemp.CreateCell(14).SetCellValue(tlst[i].OptionCPicNum);//选项C图片
                    rowtemp.CreateCell(15).SetCellValue(tlst[i].OptionD);//选项D
                    rowtemp.CreateCell(16).SetCellValue(tlst[i].OptionDPicNum);//选项D图片
                    rowtemp.CreateCell(17).SetCellValue(tlst[i].OptionE);//选项E
                    rowtemp.CreateCell(18).SetCellValue(tlst[i].OptionEPicNum);//选项E图片
                    rowtemp.CreateCell(19).SetCellValue(tlst[i].OptionF);//选项F
                    rowtemp.CreateCell(20).SetCellValue(tlst[i].OptionFPicNum);//选项F图片
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

        //public void GetSubjectList(string typename, string subjectname)
        //{
        //    if (!string.IsNullOrEmpty(typename))
        //    {
        //        ListExamSubject = Data.ExamRule.GetSubjectList(typename);
        //    }
        //    if (!string.IsNullOrEmpty(subjectname))
        //    {
        //        var c = Data.ExamBank.Get_All_ExamBank_ExamType_Subject(typename, subjectname, "","");
        //        ListTopicLevel = c.GroupBy(x => x.TopicLevel).Select(y => new ExamBank { TopicLevel = y.Key }).ToList();
        //    }
        //}
        public void GetBankInfo(SearchBnakData data)
        {
            try
            {
                if (data != null)
                {
                    var info = Data.ExamBank.Get_All_ExamBank_ExamType_Subject(data.ExamType, data.ExamSubject,data.ExamMajor,data.ExamLevel,data.ExamContent);
                    LExamBank = info.Take(500).ToList();
                    BankRemark = "题库中有" + info.Count.ToString() + "道题目";
                }
                else
                {
                    var info = Data.ExamBank.Get_All_ExamBank_ExamType_Subject("","","","","");
                    LExamBank = info.Take(500).ToList();
                    BankRemark = "题库中有" + info.Count.ToString() + "道题目";
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
        public int DeleteBankInfo( SearchBnakData data)
        {
            try
            {
                var deletecount = 0;
                if (data != null)
                {
                    if (data.ID != 0)
                    {
                        deletecount = Data.ExamBank.Delete_ExamBank(data.ID);
                    }
                    else
                    {
                        deletecount = Data.ExamBank.Delete_ExamBank_TypeName_ExamSubject_Level(data.ExamType, data.ExamSubject, data.ExamLevel, data.ExamContent);
                    }
                   
                }
                GetBankInfo(data);
                return deletecount;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public void GetTopic(int id)
        {
            VExamBank = Data.ExamBank.Get_ExamBank(id);
        }
        public void SaveBankInfo(string username)
        {
            VExamBank.CreateDate = DateTime.Now;
            VExamBank.CreateUser = username;
            //if (!string.IsNullOrEmpty(VExamBank.ExamSubject))
            //{
            //    VExamBank.ExamSubject= new Regex("(?<=;) +").Replace(VExamBank.ExamSubject, "");
            //}
            if (VExamBank.ID == 0)
            {
                Data.ExamBank.Insert_ExamBank(VExamBank, null, new string[] { "ID" });
                //Data.ExamType.Insert_ExamType(VexamType);
            }
            else
            {
                Data.ExamBank.Update_ExamBank(VExamBank, null, new string[] { "ID" });
            }
            LExamBank = Data.ExamBank.Get_All_ExamBank();
        }
        }
    public class SearchBnakData
    {
        public string ExamType { get; set; }
        public string ExamSubject { get; set; }
        public string ExamLevel { get; set; }
        public string ExamContent { get; set; }
        public string ExamMajor { get; set; }
        public int ID { get; set; }
    }
}