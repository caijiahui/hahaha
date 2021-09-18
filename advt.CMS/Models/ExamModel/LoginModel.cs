using advt.Entity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class LoginModel
    {
       public string token { get; set; }
        
        public LoginModel() : base()
        {
        }
        }
}