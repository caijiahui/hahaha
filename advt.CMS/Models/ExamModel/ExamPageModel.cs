//using advt.Entity;
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
//using NPOI.XSSF.UserModel;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Web;

//namespace advt.CMS.Models.ExamPageModel
//{
//    public class ExamPageViewModel 
//    {
//        public ExamRule Current { get; set; }
//        public ExamView examList { get; set; }
//        public List<ExamView> ListBankView { get; set; }
//        public int nowItem { get; set; }
//        public int total { get; set; }
//        public string PreNext { get; set; }
//        public ExamPageViewModel() : base()
//        {
//            examList = new ExamView();
//            ListBankView = new List<ExamView>();
//        }
//        public void GetListExam()
//        {
//            try
//            {
//                var Rule = Data.ExamRule.Get_ExamRule("EX0000001");
//                if (Rule != null)
//                {
//                    var topicnum = Data.examhe
//                }
//                foreach (var item in collection)
//                {

//                }
//                var datass = Data.ExamBank.Get_All_ExamBank_ExamType_Rule();
//                var cc = this.UnitWork.ExamRule.Get();
//                //var data = this.UnitWork.ExamScoreRecord.Get().ToList();
//                var datats = this.UnitWork.ExamRule.GetByID("EX0000001");
//                var ListBank = new List<ExamTopicsBank>();
//                if (datats != null)
//                {
//                    foreach (var item in datats.HeaderRules)
//                    {
//                        var c = this.UnitWork.ExamTopicsBank.Get(x => x.TopicType == item.ExamHeader && x.MajorType == item.ExamMajor).OrderBy(y => Guid.NewGuid()).Take(item.Topic);
//                        ListBank.AddRange(c);
//                    }

//                }
//                ListBank = ListBank.OrderBy(y => Guid.NewGuid()).ToList();
//                var model = this.UnitWork.ExamTopicsBank.Get().ToList();
//                total = ListBank.Count();
//                nowItem = 1;
//                var index = 1;
//                var ans = string.Empty;
//                var s = new string[] { };
//                foreach (var item in ListBank)
//                {
//                    List<LAnsower> answ = new List<LAnsower>();
//                    if (!string.IsNullOrEmpty(item.OptionA))
//                    {
//                        answ.Add(new LAnsower
//                        {
//                            ansower = item.OptionA,
//                            ansowerflag = "A",
//                            IsRight = item.Answer.Contains("A") ? true : false
//                        });
//                    }
//                    if (!string.IsNullOrEmpty(item.OptionB))
//                    {
//                        answ.Add(new LAnsower
//                        {
//                            ansower = item.OptionB,
//                            ansowerflag = "B",
//                            IsRight = item.Answer.Contains("B") ? true : false
//                        });
//                    }
//                    if (!string.IsNullOrEmpty(item.OptionC))
//                    {
//                        answ.Add(new LAnsower
//                        {
//                            ansower = item.OptionC,
//                            ansowerflag = "C",
//                            IsRight = item.Answer.Contains("C") ? true : false
//                        });
//                    }
//                    if (!string.IsNullOrEmpty(item.OptionD))
//                    {
//                        answ.Add(new LAnsower
//                        {
//                            ansower = item.OptionD,
//                            ansowerflag = "D",
//                            IsRight = item.Answer.Contains("D") ? true : false
//                        });
//                    }
//                    if (!string.IsNullOrEmpty(item.OptionE))
//                    {
//                        answ.Add(new LAnsower
//                        {
//                            ansower = item.OptionE,
//                            ansowerflag = "E",
//                            IsRight = item.Answer.Contains("E") ? true : false
//                        });
//                    }
//                    ListBankView.Add(new ExamView
//                    {
//                        id = item.ID,
//                        type = Convert.ToInt32(item.TopicType),
//                        proName = item.TopicName,
//                        ansowerList = answ,
//                        index = index,
//                        ansower = ans

//                    });
//                    index++;
//                }
//                if (ListBankView.Count() != 0)
//                {
//                    examList = ListBankView.OrderBy(x => x.index).FirstOrDefault();
//                }
//            }
//            catch (Exception ex)
//            {

//                throw;
//            }


//        }
//        public void GetExam()
//        {
//            foreach (var item in ListBankView)
//            {
//                if (item.index == nowItem)
//                {
//                    item.selectItem = examList.selectItem;
//                }
//            }
//            if (PreNext == "Pre")
//            {
//                nowItem = nowItem - 1;
//            }
//            if (PreNext == "Next")
//            {
//                nowItem = nowItem + 1;
//            }
//            examList = ListBankView.Where(x => x.index == nowItem).FirstOrDefault();

//        }
//        public void InsertResult()
//        {
//            var c = this.ListBankView;
//            foreach (var item in this.ListBankView)
//            {
//                var data = new ExamStuRecord();
//                data.TopicID = item.id;
//            }
//        }
//        public void PreExam()
//        {
//            if (examList != null)
//            {
//                nowItem = nowItem - 1;

//            }
//            examList = ListBankView.Where(x => x.index == nowItem).FirstOrDefault();

//        }
//        public override void GetValue(ExamRule data)
//        {
//            if (data == null)
//            {
//                throw new HttpException("不可为空");
//            }

//            this.Current = data;
//        }
//        public override void SetValue()
//        {

//            if (!string.IsNullOrWhiteSpace(this.Current.ID))
//            {
//                this.Current.ID = this.Current.ID.ToUpper();
//            }
//            if (!HasEntity(this.Current.ID))
//            {
//                Entity.ID = this.UnitWork.ExamRule.NewID(x => x.ID, 6, "CNX");
//            }
//            Entity.TotalScore = this.Current.TotalScore;
//            Entity.TotalTime = this.Current.TotalTime;
//            Entity.TotalSubject = this.Current.TotalSubject;
//            Entity.PassScore = this.Current.PassScore;
//            Entity.TopicRatio = this.Current.TopicRatio;
//            Entity.IsRead = this.Current.IsRead;
//            Entity.IsRepeat = this.Current.IsRepeat;
//            Entity.StartDeac = this.Current.StartDeac;
//            Entity.EndDesc = this.Current.EndDesc;
//            Entity.CreateUser = Thread.CurrentPrincipal.Identity.Name;
//            Entity.CreateDate = DateTime.Now;
//        }
//        protected override void AddOrUpdateEventHandler(object sender, EventArgs e)
//        {
//            if (this.UnitWork.ExamRule.GetByID(this.Current.ID) != null)
//                throw new HttpException(608, "ID不可为空");
//        }
//    }
//    public class ExamView
//    {
//        public string id { get; set; }
//        public string[] selectItem { get; set; } //用户答案 type为0时 类型为''   type为1时 类型为[]   type为2时 类型为''
//        public int type { get; set; }//0 单选题  1 多选题  2 问答题
//        public string proName { get; set; }
//        public string ansower { get; set; }
//        public List<LAnsower> ansowerList { get; set; }
//        public int index { get; set; }

//        //selectItem: '',   //用户答案 type为0时 类型为''   type为1时 类型为[]   type为2时 类型为''
//        //            type: 0,      //0 单选题  1 多选题  2 问答题
//        //            proName: '在 HTML 页面中包含一个按钮控件 mybutton ，如果要实现点击该按钮时调用已定义的 JavaScript 函数 compute ，要编写的 HTML 代码是()',
//        //            ansower: 4,     //当前题目正确答案
//        //            ansowerList: ['<input name=”mybutton” type=”button” onBlur=”compute()” value=”计算”>', '<input name=”mybutton” type=”button” onFocus=”compute()” value=”计算”>', '<input name=”mybutton” type=”button” onClick=”function compute()” value=”计算”>', '<input name=”mybutton” type=”button” onClick=”compute()” value=”计算”>']
//    }
//    public class LAnsower
//    {
//        public string ansower { get; set; }
//        public bool IsRight { get; set; }
//        public string ansowerflag { get; set; }
//    }
//}