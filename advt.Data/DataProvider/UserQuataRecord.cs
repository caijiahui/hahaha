using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial  class UserQuataRecord
    {
        public static List<Entity.UserQuataRecord> Get_All_UserQuataRecord(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_UserQuataRecord(objparams);
            return SqlHelper.GetReaderToList<Entity.UserQuataRecord>(reader);
        }

        public static List<Entity.UserQuataRecord> Get_All_UserQuataRecord()
        {
            return Get_All_UserQuataRecord(null);
        }

        public static Entity.UserQuataRecord Get_UserQuataRecord(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_UserQuataRecord(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.UserQuataRecord>(reader);
        }

        public static Entity.UserQuataRecord Get_UserQuataRecord(int ExamID)
        {
            return Get_UserQuataRecord(new { ExamID = ExamID });
        }

        public static int Insert_UserQuataRecord(Entity.UserQuataRecord info)
        {
            return Insert_UserQuataRecord(info, null, null);
        }

        public static int Insert_UserQuataRecord(Entity.UserQuataRecord info, string[] Include)
        {
            return Insert_UserQuataRecord(info, Include, null);
        }

        public static int Insert_UserQuataRecord(Entity.UserQuataRecord info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_UserQuataRecord(info, Include, Exclude);
        }
    }
}
