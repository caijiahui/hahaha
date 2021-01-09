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
            VexamType.CreateDate = DateTime.Now;
            VexamType.CreateUser = username;
            if (VexamType.ID==0)
            {
                // null, new string[] { "id" }
                VexamType.TypeName = ExamName;
                Data.ExamType.Insert_ExamType(VexamType,null, new string[] { "ID" });
                //Data.ExamType.Insert_ExamType(VexamType);
            }
            else
            {
                Data.ExamType.Update_ExamType(VexamType,null,new string[] { "ID"});
            }
            LexamType = Data.ExamType.Get_All_ExamType();
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