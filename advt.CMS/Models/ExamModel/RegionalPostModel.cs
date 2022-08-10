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
        public List<KeyValuePair<string, string>> LPostExamEntry { get; set; }
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
        }
        public void GetPostName(string RuleName, string PostName,string RuleTwoName)
        {
            ListArea = Data.AreaPost.Get_All_AreaPostArea();
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
            LPostExamEntry.Add(new KeyValuePair<string, string>("0", "-请选择-"));
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
                    var rlist = Data.RegionalPost.Get_All_RegionalPost(new { ID= VregionalPost.ID });
                    VregionalPost.CreateDate = rlist.FirstOrDefault().CreateDate;
                    VregionalPost.CreateUser = rlist.FirstOrDefault().CreateUser;
                    VregionalPost.UpdateUser = username;
                    VregionalPost.UpdateDate = DateTime.Now;
                    Data.RegionalPost.Update_RegionalPost(VregionalPost, null, new string[] { "ID" });
                }
               
            }
            else
            {
                VregionalPost.CreateUser = username;
                VregionalPost.CreateDate = DateTime.Now;
                VregionalPost.UpdateUser = username;
                VregionalPost.UpdateDate = DateTime.Now;
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
    }
}