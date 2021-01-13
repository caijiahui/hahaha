using advt.Entity;
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
    public class ExamBankModel
    {
        public ExamBankModel() : base()
        {
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
                            where dr[0] != DBNull.Value && !String.IsNullOrWhiteSpace(dr[0].ToString())
                            select new Entity.ExamBank
                            {
                                ExamType = dr[1].ToString().Trim(),
                                TopicMajor = dr[2].ToString().Trim(),
                                TopicLevel = dr[3].ToString().Trim(),
                                TopicType = dr[4].ToString().Trim(),
                                TopicTitle = dr[5].ToString().Trim(),
                                TopicTitlePicNum = dr[6].ToString().Trim(),
                                RightKey = dr[7].ToString().Trim(),
                                Remark = dr[8].ToString().Trim(),
                                OptionA = dr[9].ToString().Trim(),
                                OptionAPicNum = dr[10].ToString().Trim(),
                                OptionB = dr[11].ToString().Trim(),
                                OptionBPicNum = dr[12].ToString().Trim(),
                                OptionC = dr[13].ToString().Trim(),
                                OptionCPicNum = dr[14].ToString().Trim(),
                                OptionD = dr[15].ToString().Trim(),
                                OptionDPicNum = dr[16].ToString().Trim(),
                                OptionE = dr[17].ToString().Trim(),
                                OptionEPicNum = dr[18].ToString().Trim(),
                                OptionF = dr[19].ToString().Trim(),
                                OptionFPicNum = dr[20].ToString().Trim()
                            };
                    LBank = q.ToList();
                }
                foreach (var item in LBank)
                {
                    Data.ExamBank.Insert_ExamBank(item, null, new string[] { "ID" });
                }

            }

            catch (Exception ex)
            {
                files.Close();//关闭当前流并释放资源
            }
        }
        }
}