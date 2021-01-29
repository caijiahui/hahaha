using advt.Entity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace advt.CMS.Models
{
    public class ExamPageModel
    {
        public ExamRule Current { get; set; }
        public ExamView examList { get; set; }
        public List<ExamView> ListBankView { get; set; }
        public int nowItem { get; set; }
        public int total { get; set; }
        public string PreNext { get; set; }
        public ExamPageModel() : base()
        {
            examList = new ExamView();
            ListBankView = new List<ExamView>();
        }
        public void GetListExam()
        {
            try
            {
                var ListBanks = new List<ExamBank>();
                var Rule = Data.ExamRule.Get_ExamRule(26);
                if (Rule != null)
                {
                    var topicnum = Data.ExamRuleTopicType.Get_All_ExamRuleTopicType_RuleId(Rule.ID);
                    foreach (var item in topicnum)
                    {
                        var banks = Data.ExamBank.Get_All_ExamBank_ExamType_Rule(item.TopicType, item.TopicMajor, item.TopicLevel);
                        int TopicNum = Convert.ToInt32(item.TopicNum);
                        var bank=banks.OrderBy(y => Guid.NewGuid()).Take(TopicNum);
                        ListBanks.AddRange(bank);
                    }
                }
                total = ListBanks.Count();
                nowItem = 1;
                var index = 1;
                var ans = string.Empty;
                foreach (var item in ListBanks)
                {
                    List<LAnsower> answ = new List<LAnsower>();
                    if (!string.IsNullOrEmpty(item.OptionA))
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionA,
                            ansowerflag = "A",
                            IsRight = item.RightKey.Contains("A") ? true : false
                        });
                    }
                    if (!string.IsNullOrEmpty(item.OptionB))
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionB,
                            ansowerflag = "B",
                            IsRight = item.RightKey.Contains("B") ? true : false
                        });
                    }
                    if (!string.IsNullOrEmpty(item.OptionC))
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionC,
                            ansowerflag = "C",
                            IsRight = item.RightKey.Contains("C") ? true : false
                        });
                    }
                    if (!string.IsNullOrEmpty(item.OptionD))
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionD,
                            ansowerflag = "D",
                            IsRight = item.RightKey.Contains("D") ? true : false
                        });
                    }
                    if (!string.IsNullOrEmpty(item.OptionE))
                    {
                        answ.Add(new LAnsower
                        {
                            ansower = item.OptionE,
                            ansowerflag = "E",
                            IsRight = item.RightKey.Contains("E") ? true : false
                        });
                    }
                    ListBankView.Add(new ExamView
                    {
                        id = item.ID.ToString(),
                        type = item.TopicType,
                        proName = item.TopicTitle,
                        ansowerList = answ,
                        index = index,
                        ansower = ans

                    });
                    index++;
                }
                if (ListBankView.Count() != 0)
                {
                    examList = ListBankView.OrderBy(x => x.index).FirstOrDefault();
                }
            
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public void GetExam()
        {
            foreach (var item in ListBankView)
            {
                if (item.index == nowItem)
                {
                    item.selectItem = examList.selectItem;
                }
            }
            if (PreNext == "Pre")
            {
                nowItem = nowItem - 1;
            }
            if (PreNext == "Next")
            {
                nowItem = nowItem + 1;
            }
            examList = ListBankView.Where(x => x.index == nowItem).FirstOrDefault();

        }
        public void InsertResult()
        {
            var c = this.ListBankView;
            foreach (var item in this.ListBankView)
            {
                //var data = new ExamStuRecord();
                //data.TopicID = item.id;
            }
        }
        public void PreExam()
        {
            if (examList != null)
            {
                nowItem = nowItem - 1;

            }
            examList = ListBankView.Where(x => x.index == nowItem).FirstOrDefault();

        }
    }
    public class ExamView
    {
        public string id { get; set; }
        public string[] selectItem { get; set; } //用户答案 type为0时 类型为''   type为1时 类型为[]   type为2时 类型为''
        public string type { get; set; }//0 单选题  1 多选题  2 问答题
        public string proName { get; set; }
        public string ansower { get; set; }
        public List<LAnsower> ansowerList { get; set; }
        public int index { get; set; }

        //selectItem: '',   //用户答案 type为0时 类型为''   type为1时 类型为[]   type为2时 类型为''
        //            type: 0,      //0 单选题  1 多选题  2 问答题
        //            proName: '在 HTML 页面中包含一个按钮控件 mybutton ，如果要实现点击该按钮时调用已定义的 JavaScript 函数 compute ，要编写的 HTML 代码是()',
        //            ansower: 4,     //当前题目正确答案
        //            ansowerList: ['<input name=”mybutton” type=”button” onBlur=”compute()” value=”计算”>', '<input name=”mybutton” type=”button” onFocus=”compute()” value=”计算”>', '<input name=”mybutton” type=”button” onClick=”function compute()” value=”计算”>', '<input name=”mybutton” type=”button” onClick=”compute()” value=”计算”>']
    }
    public class LAnsower
    {
        public string ansower { get; set; }
        public bool IsRight { get; set; }
        public string ansowerflag { get; set; }
    }
}