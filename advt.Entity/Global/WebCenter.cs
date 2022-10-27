using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Entity.Global
{
    public class WebCenter
    { 
        public string System { get; set; }
        public string Param { get; set; }
        public bool Authorized { get; set; }
        public MSginfo UserMsg { get; set; }
    }
    public class MSginfo
    { 
        public string OPID { get; set; }
        public string ChineseName { get; set; }
        public string ForeignName { get; set; }
        public string Domain { get; set; }
        public string DomainAccount { get; set; }
        public string Email { get; set; }
    }
}
