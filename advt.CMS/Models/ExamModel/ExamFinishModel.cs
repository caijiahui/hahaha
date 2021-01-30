using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using advt.Entity;

namespace advt.CMS.Models
{
    public class ExamFinishModel
    {
        public ExamRecord VexamRecord { get; set; }
        public ExamScore VexamScore { get; set; }
        public ExamContent ExamContentInfo { get; set; }
        public List<ExamContent> ListExamContent { get; set; }
        public decimal totalGrade { get; set; }
        public decimal grade { get; set; }
        public int rightItem { get; set; }
        public int total { get; set; }
        public ExamContent examList { get; set; }
        public ExamFinishModel() : base()
        {
            VexamRecord = new ExamRecord();
            VexamScore = new ExamScore();
            examList = new ExamContent();
        }
    }
    public class ExamContent
    {
        public string id { get; set; }
        public string[] selectItem { get; set; } //用户答案 type为0时 类型为''   type为1时 类型为[]   type为2时 类型为''
        public string type { get; set; }//0 单选题  1 多选题  2 问答题
        public string proName { get; set; }
        public string ansower { get; set; }
        public List<LAnsower> ansowerList { get; set; }
        public int index { get; set; }

         }
}