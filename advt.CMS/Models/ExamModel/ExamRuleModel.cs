﻿using advt.Entity;
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
        public List<ExamRuleTopicType> RuleGrList { get; set; }
        public List<ExamRuleTopicType> RuleTopicList { get; set; }
        public List<ExamRule> ListExamRule { get; set; }

        public TopicInfo TopicInfoList { get; set; }
        public List<TopicInfo> ListTopic{ get; set; }
        public List<ExamBank> ListTopicInfo { get; set; }
        public List<ExamSubject> ListExamSubject { get; set; }
        public ExamRuleTopicType topictype { get; set; }
        public ExamRuleModel() : base()
        {
            VExamRule = new ExamRule();
            ListExamRule = Data.ExamRule.Get_All_ExamRule();
            ListExamSubject = new List<ExamSubject>();
            TopicInfoList = new TopicInfo();
            ListTopicInfo = new List<ExamBank>();
            ListTopic = new List<TopicInfo>();
            RuleGrList = new List<ExamRuleTopicType>();
            topictype = new ExamRuleTopicType();
            RuleTopicList = new List<ExamRuleTopicType>();
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
        public void SaveTopicInfo(int id)
        {
            ExamRuleTopicType type= new ExamRuleTopicType();
            if (RuleGrList != null)
            {
                foreach (var item in RuleGrList)
                {
                    if (id != 0)
                    {
                        var ee = Data.ExamRuleTopicType.Get_ExamRuleInfo(item.TopicLevel, item.TopicMajor, item.TopicType, id);
                        type.TopicLevel = item.TopicLevel;
                        type.TopicMajor = item.TopicMajor;
                        type.TopicType = item.TopicType;
                        type.RuleId = id;
                        if (ee.Count() > 0)
                        {
                            type.ID = item.ID;
                            type.TopicScore = item.TopicScore;
                            type.TopicNum = item.TopicNum;
                            Data.ExamRuleTopicType.Update_ExamRuleTopicType(type, null, new string[] { "ID" });
                        }
                        else
                        {
                            type.TopicScore = item.TopicScore;
                            type.TopicNum = item.TopicNum;
                            Data.ExamRuleTopicType.Insert_ExamRuleTopicType(type, null, new string[] { "ID" });

                        }
                        
                    }
                }
            }

        }
        public void Delete_ExamRule(int model)
        {
            Data.ExamRule.Delete_ExamRule(model);
            ListExamRule = Data.ExamRule.Get_All_ExamRule();

        }
        public void GetSubjectList(string model)
        {
            ListExamSubject= Data.ExamRule.GetSubjectList(model);
            ListTopicInfo = Data.ExamRule.GetTopicInfo(model);

            int ss = 0;
            foreach (var item in ListTopicInfo)
            {
                ss++;
                ListTopic.Add(new TopicInfo
                {
                    ExamType=item.ExamType,
                    TopicMajor=item.TopicMajor,
                    TopicLevel=item.TopicLevel,
                    TopicNum="0",
                    TopicScore=0,
                    TopicType=item.TopicType,
                    ID=ss
                });
            }
        
        }
        public void GetRuleType(string model)
        {
            if (model != null)
            {
                RuleTopicList= Data.ExamRuleTopicType.Get_ExamRuleTopic(model);
            }
        }
    }
    public class TopicInfo
    {
        public string ExamType { get; set; }//考试类型
        public string TopicMajor { get; set; }//题目类型
        public string TopicLevel { get; set; }//题目等级
        public string TopicType { get; set; }
        public string TopicNum { get; set; }

        public decimal? TopicScore { get; set; }

        public int? RuleId { get; set; }
        public int ID { get; set; }
    }
}