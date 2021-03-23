using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class ImportCheckEmail
    {
        public string Title { get; set; }

        public string Content { get; set; }
        public List<CheckedEmail> ListCheckedEmail { get; set; }

        public List<CheckedItems> ListCheckedItems { get; set; }

        public ImportCheckEmail() : base()
        {
            ListCheckedEmail = new List<CheckedEmail>();
            ListCheckedItems = new List<CheckedItems>();
        }

       
    }
    public class CheckedItems
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
    public class CheckedEmail
    {
        public string Name { get; set; }
    }
} 