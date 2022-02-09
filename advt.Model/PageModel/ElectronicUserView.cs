using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using advt.Entity;

namespace advt.Model.ExamModel
{
    public class ElectronicUserView
    {
        public string SubjectName { get; set; }
        public int HC { get; set; }
        public int CurrentNumber { get; set; }
        public int Ratio { get; set; }//比例
        public int Vacancy { get; set; }//缺额
        public int ElectronicQuota { get; set; }//津贴额度


    }
}