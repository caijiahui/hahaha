using advt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class ExamSubjectModel
    {
        public ExamSubject VexamSubject { get; set; }
        public List<ExamSubject> ListExamSubject { get; set; }
        public ExamSubjectModel() : base()
        {
            VexamSubject = new ExamSubject();
            ListExamSubject = Data.ExamSubject.Get_All_ExamSubject();
        }
        public void SaveSubject(string username)
        {
            if (VexamSubject.ID!=0)
            {

                VexamSubject.CreateUser = username;
                VexamSubject.CreateDate = DateTime.Now;
                Data.ExamSubject.Update_ExamSubject(VexamSubject, null, new string[] { "ID" });
            }
            else
            {

                VexamSubject.CreateUser = username;
                VexamSubject.CreateDate = DateTime.Now;
                Data.ExamSubject.Insert_ExamSubject(VexamSubject, null, new string[] { "ID" });
            }
            ListExamSubject = Data.ExamSubject.Get_All_ExamSubject();
        }
        public void Delete_ExamSubject(int model)
        {
            Data.ExamSubject.Delete_ExamSubject(model);
            ListExamSubject = Data.ExamSubject.Get_All_ExamSubject();
        }
    }
}