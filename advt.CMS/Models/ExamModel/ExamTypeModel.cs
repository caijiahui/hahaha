using advt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class ExamTypeModel
    {
        public ExamType VexamType { get; set; }
        public List<ExamType> LexamType { get; set; }
        public string ExamName { get; set; }
        public string Result { get; set; }
        public ExamTypeModel() : base()
        {
            VexamType = new ExamType();
            LexamType = new List<ExamType>();
        }
        public void GetAllVexamType()
        {
            LexamType= Data.ExamType.Get_All_ExamType();
        }
        public void SaveType(string username)
        {
            try
            {
                VexamType.CreateDate = DateTime.Now;
                VexamType.CreateUser = username;
                if (VexamType.ID == 0)
                {
                    var repeat = Data.ExamType.Get_ExamType(new { TypeName = ExamName });
                    if (repeat != null)
                    {
                        Result = "考试类型不可重复添加";
                    }
                    else
                    {
                        VexamType.TypeName = ExamName;
                        Data.ExamType.Insert_ExamType(VexamType, null, new string[] { "ID" });
                    }

                }
                else
                {
                    Data.ExamType.Update_ExamType(VexamType, null, new string[] { "ID" });
                }
                LexamType = Data.ExamType.Get_All_ExamType();
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public void DeleteType(string ID)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                var ids = Convert.ToInt32(ID);
                Data.ExamType.Delete_ExamType(ids);
            }
            LexamType = Data.ExamType.Get_All_ExamType();
        }
    }
}