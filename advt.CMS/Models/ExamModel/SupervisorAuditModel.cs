using advt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class SupervisorAuditModel
    {
        public List<ExamUserDetailInfo> ListExamUserDetailInfos{ get; set; }
        public SupervisorAuditModel() : base()
        {
            ListExamUserDetailInfos = new List<ExamUserDetailInfo>();
        }
        public void GetAllExamUserDetailInfo()
        {
            //1.拼接出明细表的拓展表，现在没有，先直接用明细表替代
            var c = Data.ExamUserDetailInfo.Get_ExamUserDetailInfo(1);
            ListExamUserDetailInfos.Add(c);

        }
    }
}