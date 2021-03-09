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
        public List<ExamRuleTopicType> RuleGrList { get; set; }
        public List<ExamRuleTopicType> RuleTopic { get; set; }
        public List<ExamRuleTopicType> RuleTopicList { get; set; }
        public List<ExamRule> ListExamRule { get; set; }

        public TopicInfo TopicInfoList { get; set; }
        public List<TopicInfo> ListTopic{ get; set; }
        public List<ExamBank> ListTopicInfo { get; set; }
        public List<ExamSubject> ListExamSubject { get; set; }
        public ExamRuleTopicType topictype { get; set; }
        public List<ExamRule> ListExamRuleInfo { get; set; }
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
            RuleTopic = new List<ExamRuleTopicType>();
            ListExamRuleInfo = new List<ExamRule>();
        }
       

        public string SaveRuleInfo(string username)
        {
            var Result = "";
            if (VExamRule.ID!=0)
            {
                ListExamRuleInfo = Data.ExamRule.Get_All_ExamRuleInfo(VExamRule.RuleName);
                if (ListExamRuleInfo.Count() > 0 && ListExamRuleInfo != null)
                {
                    Result += VExamRule.RuleName + "此考试规则已存在";
                }
                else
                {
                    VExamRule.CreateUser = username;
                    VExamRule.CreateDate = DateTime.Now;
                    Data.ExamRule.Update_ExamRule(VExamRule, null, new string[] { "ID" });
                }
            }
            else
            {
                ListExamRuleInfo = Data.ExamRule.Get_All_ExamRuleInfo(VExamRule.RuleName);
                if (ListExamRuleInfo.Count() > 0 && ListExamRuleInfo != null)
                {
                    Result += VExamRule.RuleName + "此考试规则已存在";
                }
                else
                {
                    VExamRule.CreateUser = username;
                    VExamRule.CreateDate = DateTime.Now;
                    Data.ExamRule.Insert_ExamRule(VExamRule, null, new string[] { "ID" });
                }

               
            }
            
            ListExamRule = Data.ExamRule.Get_All_ExamRule();
            return Result;
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
                        if (item.TopicType == "单选")
                        {
                            type.TopicType = "0";
                        }
                        else if (item.TopicType == "问答")
                        {
                            type.TopicType = "2";
                        }
                        else if (item.TopicType == "多选")
                        {
                            type.TopicType = "1";
                        }
                        var ee = Data.ExamRuleTopicType.Get_ExamRuleInfo(item.TopicLevel, item.TopicMajor, type.TopicType, id);
                        type.TopicLevel = item.TopicLevel;
                        type.TopicMajor = item.TopicMajor;
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
                var type = "";
                if (item.TopicType == "0")
                {
                    type = "单选";
                }
                else if (item.TopicType == "2")
                {
                    type = "问答";
                }
                else if (item.TopicType == "1")
                {
                    type ="多选";
                }
                ss++;
                ListTopic.Add(new TopicInfo
                {
                    ExamType=item.ExamType,
                    TopicMajor=item.TopicMajor,
                    TopicLevel=item.TopicLevel,
                    TopicNum="0",
                    TopicScore=0,
                    TopicType= type,
                    ID=ss
                });
            }

        }
        public void GetRuleSubjectList(string model)
        {
            ListTopicInfo = Data.ExamRule.GetTRuleSubjectInfo(model);

            int ss = 0;
            foreach (var item in ListTopicInfo)
            {
                var type = "";
                if (item.TopicType == "0")
                {
                    type = "单选";
                }
                else if (item.TopicType == "2")
                {
                    type = "问答";
                }
                else if (item.TopicType == "1")
                {
                    type = "多选";
                }
                ss++;
                ListTopic.Add(new TopicInfo
                {
                    ExamType = item.ExamType,
                    TopicMajor = item.TopicMajor,
                    TopicLevel = item.TopicLevel,
                    TopicNum = "0",
                    TopicScore = 0,
                    TopicType = type,
                    ID = ss
                });
            }

        }

        
        public void GetRuleType(string model)
        {
            if (model != null)
            {
                RuleTopic  = Data.ExamRuleTopicType.Get_ExamRuleTopic(model);
                if (RuleTopic.Count() > 0)
                {
                    foreach (var item in RuleTopic)
                    {
                        var type = "";
                        if (item.TopicType == "0")
                        {
                            type = "单选";
                        }
                        else if (item.TopicType == "2")
                        {
                            type = "问答";
                        }
                        else if (item.TopicType == "1")
                        {
                            type = "多选";
                        }
                        RuleTopicList.Add(new ExamRuleTopicType {
                            TopicMajor = item.TopicMajor,
                            TopicLevel = item.TopicLevel,
                            TopicNum = item.TopicNum,
                            TopicScore =item.TopicScore,
                            TopicType = type,
                            ID = item.ID,
                            RuleId=item.RuleId
                        });
                    }
                }
               
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