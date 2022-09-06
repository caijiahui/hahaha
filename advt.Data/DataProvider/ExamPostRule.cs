using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial class ExamPostRule
    {
        public static List<Entity.ExamPostRule> Get_All_ExamPostRule(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamPostRule(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamPostRule>(reader);
        }

        public static List<Entity.ExamPostRule> Get_All_ExamPostRule()
        {
            return Get_All_ExamPostRule(null);
        }

        public static Entity.ExamPostRule Get_ExamPostRule(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamPostRule(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamPostRule>(reader);
        }


        public static int Insert_ExamPostRule(Entity.ExamPostRule info)
        {
            return Insert_ExamPostRule(info, null, null);
        }

        public static int Insert_ExamPostRule(Entity.ExamPostRule info, string[] Include)
        {
            return Insert_ExamPostRule(info, Include, null);
        }

        public static int Insert_ExamPostRule(Entity.ExamPostRule info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamPostRule(info, Include, Exclude);
        }

        public static int Update_ExamPostRule(Entity.ExamPostRule info)
        {
            return Update_ExamPostRule(info, null, null);
        }

        public static int Update_ExamPostRule(Entity.ExamPostRule info, string[] Include)
        {
            return Update_ExamPostRule(info, Include, null);
        }

        public static int Update_ExamPostRule(Entity.ExamPostRule info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamPostRule(info, Include, Exclude);
        }

        public static int Delete_ExamPostRule(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamPostRule(ID);
        }

    }
}
