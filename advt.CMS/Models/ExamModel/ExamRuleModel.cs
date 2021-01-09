using advt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace advt.CMS.Models.ExamModel
{
    public class ExamRuleModel
    {
        public ExamRule VExamRule { get; set; }

        public List<ExamRule> ListExamRule { get; set; }
        public ExamRuleModel() : base()
        {
            VExamRule = new ExamRule();
            ListExamRule = Data.ExamRule.Get_All_ExamRule();
        }
       

        public void SaveRuleInfo(string username)
        {
            if (VExamRule.ID!=0)
            {

                VExamRule.CreateUser = username;
                VExamRule.CreateDate = DateTime.Now;
                Data.ExamRule.Update_ExamRule(VExamRule, null, new string[] { "ID" });
            }
            else
            {

                VExamRule.CreateUser = username;
                VExamRule.CreateDate = DateTime.Now;
                Data.ExamRule.Insert_ExamRule(VExamRule, null, new string[] { "ID" });
            }
            ListExamRule = Data.ExamRule.Get_All_ExamRule();
        }
        public void Delete_ExamRule(int model)
        {
            Data.ExamRule.Delete_ExamRule(model);
            ListExamRule = Data.ExamRule.Get_All_ExamRule();

        }
    }
}