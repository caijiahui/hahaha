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
        public List<ExamSubject> ListExamSubjectName { get; set; }
        public ExamSubjectModel() : base()
        {
            VexamSubject = new ExamSubject();
            ListExamSubject = Data.ExamSubject.Get_All_ExamSubject();
            ListExamSubjectName = new List<ExamSubject>();
        }
        public string  SaveSubject(string username)
        {
            var Result = "";
            if (VexamSubject.ID!=0)
            {
                ListExamSubjectName = Data.ExamSubject.Get_All_ExamSubjectInfo(VexamSubject.SubjectName, VexamSubject.TypeName);
                if (ListExamSubjectName.Count() > 0 && ListExamSubjectName != null)
                {
                    Result += VexamSubject.SubjectName + "此考试类型下的科目已存在";
                }
                else
                {

                    VexamSubject.CreateUser = username;
                    VexamSubject.CreateDate = DateTime.Now;
                    Data.ExamSubject.Update_ExamSubject(VexamSubject, null, new string[] { "ID" });
                }
            }
            else
            {
                ListExamSubjectName = Data.ExamSubject.Get_All_ExamSubjectInfo( VexamSubject.SubjectName, VexamSubject.TypeName);
                if (ListExamSubjectName.Count() > 0&& ListExamSubjectName != null)
                {
                    Result += VexamSubject.SubjectName + "此考试类型下的科目已存在";
                }
                else
                {
                    VexamSubject.CreateUser = username;
                    VexamSubject.CreateDate = DateTime.Now;
                    Data.ExamSubject.Insert_ExamSubject(VexamSubject, null, new string[] { "ID" });
                }
               
            }
            ListExamSubject = Data.ExamSubject.Get_All_ExamSubject();
            return Result;
        }
        public void Delete_ExamSubject(int model)
        {
            Data.ExamSubject.Delete_ExamSubject(model);
            ListExamSubject = Data.ExamSubject.Get_All_ExamSubject();
        }
    }
}