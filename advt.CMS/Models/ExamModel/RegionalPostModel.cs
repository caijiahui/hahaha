﻿using advt.Entity;
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
        public RegionalPostModel() : base()
        {
            VregionalPost = new RegionalPost();
            ListRegionalPost = new List<RegionalPost>();
            LExamRule = new List<KeyValuePair<string, string>>();
        }
        public void GetPostName(string RuleName, string PostName)
        {
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPostInfo(RuleName,PostName);
            LExamRule.Add(new KeyValuePair<string, string>("", "-全部-"));
            foreach (var item in Data.ExamRule.Get_All_ExamRule())
            {
                LExamRule.Add(new KeyValuePair<string, string>(item.RuleName, item.RuleName));
            }
        }
        public string SavePostInfo(string username)
        {
            var Result = "";
            if (VregionalPost.ID != 0)
            {
                if (!string.IsNullOrEmpty(VregionalPost.PostName))
                {
                    Data.RegionalPost.Update_RegionalPostInfo(VregionalPost.PostName, VregionalPost.RuleName);
                }
               
            }
            else
            {
                Data.RegionalPost.Insert_RegionalPost(VregionalPost, null, new string[] { "ID" });
            }
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPost();
            return Result;
        }
        public void Delete_ExamPost(int model)
        {
            Data.RegionalPost.Delete_RegionalPost(model);
            ListRegionalPost = Data.RegionalPost.Get_All_RegionalPost();
        }
    }
}