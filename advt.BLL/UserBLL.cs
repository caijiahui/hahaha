using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace advt.BLL
{
    public class UserBLL
    {
        public static List<string> Get_All_L_type()
        {
            List<string> List = new List<string>();
            List.Add("HRManager");
            List.Add("SupervisorAudit");
            List.Add("Admin");
            return List;
        }
    }
}
