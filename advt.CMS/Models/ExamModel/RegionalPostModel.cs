using advt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class RegionalPostModel
    {
        public RegionalPost VregionalPost { get; set; }
        public List<RegionalPost> ListRegionalPost { get; set; }

        public List<KeyValuePair<string, string>> LExamRule { get; set; }
        public List<KeyValuePair<string, string>> LExamRuleTwo { get; set; }
        public List<KeyValuePair<string, string>> LExamType { get; set; }
        public List<KeyValuePair<string, string>> LPostType { get; set; }//岗位类型
        public List<KeyValuePair<string, string>> LPostCycleType { get; set; }//岗位周期类型
        public RegionalPostModel() : base()
        {
            VregionalPost = new RegionalPost();
            ListRegionalPost = new List<RegionalPost>();
            LExamRule = new List<KeyValuePair<string, string>>();
            LExamRuleTwo = new List<KeyValuePair<string, string>>();
            LExamType = new List<KeyValuePair<string, string>>();
            LPostType = new List<KeyValuePair<string, string>>();
            LPostCycleType = new List<KeyValuePair<string, string>>();
        }
        public void GetPostName(string RuleName, string PostName,string RuleTwoName)
        {
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPostInfo(RuleName,PostName, RuleTwoName);
            ListRegionalPost = ListRegionalPost.OrderByDescending(x => x.CreateDate).ToList();
            LExamRule.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamRule.Get_All_ExamRule())
            {
                LExamRule.Add(new KeyValuePair<string, string>(item.RuleName, item.RuleName));
            }

            LExamRuleTwo.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamRule.Get_All_ExamRule())
            {
                LExamRuleTwo.Add(new KeyValuePair<string, string>(item.RuleName, item.RuleName));
            }

            LExamType.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamType.Get_All_ExamType())
            {
                LExamType.Add(new KeyValuePair<string, string>(item.TypeName, item.TypeName));
            }
            LPostType.Add(new KeyValuePair<string, string>("", "-请选择-"));
            LPostType.Add(new KeyValuePair<string, string>("一般岗", "一般岗"));
            LPostType.Add(new KeyValuePair<string, string>("技术岗", "技术岗"));
            
            LPostCycleType.Add(new KeyValuePair<string, string>("", "-请选择-"));
            LPostCycleType.Add(new KeyValuePair<string, string>("3", "3"));
            LPostCycleType.Add(new KeyValuePair<string, string>("6", "6"));
            LPostCycleType.Add(new KeyValuePair<string, string>("9", "9"));
            LPostCycleType.Add(new KeyValuePair<string, string>("12", "12"));

        }
        public string SavePostInfo(string username)
        {
            var Result = "";
            if (VregionalPost.ID != 0)
            {
                if (!string.IsNullOrEmpty(VregionalPost.PostName))
                {
                    Data.RegionalPost.Update_RegionalPostInfo(VregionalPost.PostName, VregionalPost.RuleName, VregionalPost.RuleTwoName,VregionalPost.PostType,VregionalPost.PostCycle);
                }
               
            }
            else
            {
                VregionalPost.CreateUser = username;
                VregionalPost.CreateDate = DateTime.Now;
                Data.RegionalPost.Insert_RegionalPost(VregionalPost, null, new string[] { "ID" });
            }
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPost().OrderByDescending(x=>x.CreateDate).ToList();
            return Result;
        }
        public void Delete_ExamPost(int model)
        {
            Data.RegionalPost.Delete_RegionalPost(model);
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPost().OrderByDescending(x => x.CreateDate).ToList();
        }
    }
}