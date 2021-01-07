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
        public ExamTypeModel() : base()
        {
            VexamType = new ExamType();
        }
        public void SaveType()
        {
            var c = Data.ExamType.Update_ExamType(VexamType);
        }
    }
}