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
        public List<KeyValuePair<string, string>> LPostRank { get; set; }//职位职等
        public List<AreaPost> ListArea { get; set; }//区域
        public List<AreaPost> ListPostDepart { get; set; }
        public List<AreaPost> ListPostName { get; set; }
        public List<ExamPostRule> ListExamPostRule { get; set; }
        public List<KeyValuePair<string, string>> LPostExamEntry { get; set; }
       
        public List<ExamRule> ListExamRuleTwo { get; set; }
        public RuleView ruleView { get; set; }
        public List<RuleView> PostRuleList { get; set; }


        public RegionalPostModel() : base()
        {
            VregionalPost = new RegionalPost();
            ListRegionalPost = new List<RegionalPost>();
            LExamRule = new List<KeyValuePair<string, string>>();
            LExamRuleTwo = new List<KeyValuePair<string, string>>();
            LExamType = new List<KeyValuePair<string, string>>();
            LPostType = new List<KeyValuePair<string, string>>();
            LPostCycleType = new List<KeyValuePair<string, string>>();//是否固定考试月份
            LPostRank = new List<KeyValuePair<string, string>>();
            ListArea = new List<AreaPost>();
            ListPostDepart = new List<AreaPost>();
            ListPostName = new List<AreaPost>();
            LPostExamEntry = new List<KeyValuePair<string, string>>();
            ListExamPostRule = new List<ExamPostRule>();
            ListExamRuleTwo = new List<ExamRule>();
            ruleView = new RuleView();
            PostRuleList = new List<RuleView>();
        }
        public void GetPostName(string RuleName, string PostName,string RuleTwoName, string postid, string departcode)
        {
            ListArea = Data.AreaPost.Get_All_AreaPostArea();
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPostInfo(RuleName,PostName, RuleTwoName);
            ListRegionalPost = ListRegionalPost.OrderByDescending(x => x.CreateDate).ToList();

            var reg = Data.RegionalPost.Get_All_RegionalPost(new {PostName=PostName, DepartCode = departcode });
            if (reg.Count() > 0)
            {
                ListExamPostRule = Data.ExamPostRule.Get_All_ExamPostRule(new {PostName = PostName, DepartCode = departcode });
            }

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
            //岗位类型
            LPostType.Add(new KeyValuePair<string, string>("", "-请选择-"));
            LPostType.Add(new KeyValuePair<string, string>("一般岗", "一般岗"));
            LPostType.Add(new KeyValuePair<string, string>("技术岗", "技术岗"));

            //固定考试月份
            for (int i = 0; i <= 12; i++)
            {
                LPostCycleType.Add(new KeyValuePair<string, string>(i.ToString(), i.ToString()));
            }
            //职等
            LPostRank.Add(new KeyValuePair<string, string>("", "-请选择-"));
            LPostRank.Add(new KeyValuePair<string, string>("A职等", "A职等"));
            LPostRank.Add(new KeyValuePair<string, string>("B职等", "B职等"));
            LPostRank.Add(new KeyValuePair<string, string>("C职等", "C职等"));

            //要求入职时间判断
            LPostExamEntry.Add(new KeyValuePair<string, string>("0", "否"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("1", "每月10号前入职,下个月开始考"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("2", "每月10号后入职,下下个月开始考"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("3", "7天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("4", "14天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("5", "30天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("6", "90天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("7", "180天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("8", "1年"));
        }
        public string SavePostInfo(string username)
        {
            var Result = "";
            if (VregionalPost.ID != 0)
            {
                if (!string.IsNullOrEmpty(VregionalPost.PostName))
                {
                    var rlist = Data.RegionalPost.Get_All_RegionalPost(new { ID = VregionalPost.ID });
                    VregionalPost.CreateDate = rlist.FirstOrDefault().CreateDate;
                    VregionalPost.CreateUser = rlist.FirstOrDefault().CreateUser;
                    VregionalPost.UpdateUser = username;
                    VregionalPost.UpdateDate = DateTime.Now;
                    Data.RegionalPost.Update_RegionalPost(VregionalPost, null, new string[] { "ID" });
                }
                foreach (var item in PostRuleList)
                {
                    if (!string.IsNullOrEmpty(item.RuleName))
                    {
                        var rule = Data.ExamPostRule.Get_All_ExamPostRule(new { PostName = VregionalPost.PostName, DepartCode = VregionalPost.DepartCode, RuleName = item.RuleName });
                        if (rule.Count() > 0)
                        {
                            rule.FirstOrDefault().RuleName = item.RuleName;
                            rule.FirstOrDefault().IsRuleAch = item.IsRuleAch;
                            rule.FirstOrDefault().IsRuleQuality = item.IsRuleQuality;
                            rule.FirstOrDefault().IsRuleShangGang = item.IsRuleShangGang;
                            rule.FirstOrDefault().PostExamEntry = item.PostExamEntry;
                            rule.FirstOrDefault().UpdateUser = username;
                            rule.FirstOrDefault().UpdateDate = DateTime.Now;
                            Data.ExamPostRule.Update_ExamPostRule(rule.FirstOrDefault(), null, new string[] { "ID" });
                        }
                        else
                        {
                            var rules = new ExamPostRule();
                            rules.PostID = VregionalPost.PostID;
                            rules.PostName = VregionalPost.PostName;
                            rules.DepartCode = VregionalPost.DepartCode;
                            rules.RuleName = item.RuleName;
                            rules.IsRuleAch = item.IsRuleAch;
                            rules.IsRuleQuality = item.IsRuleQuality;
                            rules.IsRuleShangGang = item.IsRuleShangGang;
                            rules.PostExamEntry = item.PostExamEntry;
                            rules.UpdateUser = username;
                            rules.UpdateDate = DateTime.Now;
                            Data.ExamPostRule.Insert_ExamPostRule(rules, null, new string[] { "ID" });
                        }
                    }
                   
                }
               
            }
            else
            {
                VregionalPost.CreateUser = username;
                VregionalPost.CreateDate = DateTime.Now;
                VregionalPost.UpdateUser = username;
                VregionalPost.UpdateDate = DateTime.Now;
                Data.RegionalPost.Insert_RegionalPost(VregionalPost, null, new string[] { "ID" });

                foreach (var item in PostRuleList)
                {
                    if (!string.IsNullOrEmpty(item.RuleName))
                    {
                        var rule = new ExamPostRule();
                        rule.PostID = VregionalPost.PostID;
                        rule.PostName = VregionalPost.PostName;
                        rule.DepartCode = VregionalPost.DepartCode;
                        rule.RuleName = item.RuleName;
                        rule.IsRuleAch = item.IsRuleAch;
                        rule.IsRuleQuality = item.IsRuleQuality;
                        rule.IsRuleShangGang = item.IsRuleShangGang;
                        rule.PostExamEntry = item.PostExamEntry;
                        rule.UpdateUser = username;
                        rule.UpdateDate = DateTime.Now;
                        Data.ExamPostRule.Insert_ExamPostRule(rule, null, new string[] { "ID" });
                    }
                      
                }
               
            }
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPost().OrderByDescending(x=>x.CreateDate).ToList();
            return Result;
        }
        public void Delete_ExamPost(int model)
        {
            Data.RegionalPost.Delete_RegionalPost(model);
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPost().OrderByDescending(x => x.CreateDate).ToList();
        }
        public void DeleteExamPostRule(int model, string postname, string departcode)
        {
            Data.ExamPostRule.Delete_ExamPostRule(model);
            ListExamPostRule = Data.ExamPostRule.Get_All_ExamPostRule(new { PostName = postname, DepartCode = departcode });
        }
        public void GetPostDepart(string model)
        {
            ListPostDepart = Data.AreaPost.Get_All_AreaPostDept(model);
        }
        public void GetPostName(string model)
        { 
            ListPostName= Data.AreaPost.Get_All_AreaPostName(model);
        }
        public string GetPostID(string model)
        {
            var postid = string.Empty;
            postid = Data.AreaPost.Get_All_AreaPost(new { PostName = model }).FirstOrDefault()?.PostID;
            return postid;
        }
        public void GetRulePost(string model, string postname, string departcode)
        {
            ListExamPostRule = Data.ExamPostRule.Get_All_ExamPostRule(new { PostName = postname, DepartCode = departcode });
          
            //要求入职时间判断
            LPostExamEntry.Add(new KeyValuePair<string, string>("0", "否"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("1", "每月10号前入职,下个月开始考"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("2", "每月10号后入职,下下个月开始考"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("3", "7天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("4", "14天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("5", "30天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("6", "90天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("7", "180天"));
            LPostExamEntry.Add(new KeyValuePair<string, string>("8", "1年"));
        }
        public void GetExamTypeInfo(string model)
        { ListExamRuleTwo = Data.ExamRule.Get_All_ExamRule(new { TypeName = model }); }
    }
    public class RuleView
    { 
        public string RuleName { get; set; }
        public  string PostExamEntry { get; set; }
        public bool IsRuleAch { get; set; }
        public bool IsRuleQuality { get; set; }
        public bool IsRuleShangGang { get; set; }
    }
}