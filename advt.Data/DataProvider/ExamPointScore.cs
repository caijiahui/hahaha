using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advt.Data
{
    public partial class ExamPointScore
    {
        #region ExamPointScore 

        public static List<Entity.ExamPointScore> Get_All_ExamPointScore(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamPointScore(objparams);
            return SqlHelper.GetReaderToList<Entity.ExamPointScore>(reader);
        }

        public static List<Entity.ExamPointScore> Get_All_ExamPointScore()
        {
            return Get_All_ExamPointScore(null);
        }

        public static Entity.ExamPointScore Get_ExamPointScore(object objparams)
        {
            IDataReader reader = DatabaseProvider.GetInstance().Get_All_ExamPointScore(objparams);
            return SqlHelper.GetReaderToFirstOrDefault<Entity.ExamPointScore>(reader);
        }


        public static int Insert_ExamPointScore(Entity.ExamPointScore info)
        {
            return Insert_ExamPointScore(info, null, null);
        }

        public static int Insert_ExamPointScore(Entity.ExamPointScore info, string[] Include)
        {
            return Insert_ExamPointScore(info, Include, null);
        }

        public static int Insert_ExamPointScore(Entity.ExamPointScore info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Insert_ExamPointScore(info, Include, Exclude);
        }

        public static int Update_ExamPointScore(Entity.ExamPointScore info)
        {
            return Update_ExamPointScore(info, null, null);
        }

        public static int Update_ExamPointScore(Entity.ExamPointScore info, string[] Include)
        {
            return Update_ExamPointScore(info, Include, null);
        }

        public static int Update_ExamPointScore(Entity.ExamPointScore info, string[] Include, string[] Exclude)
        {
            return DatabaseProvider.GetInstance().Update_ExamPointScore(info, Include, Exclude);
        }

        public static int Delete_ExamPointScore(int ID)
        {
            return DatabaseProvider.GetInstance().Delete_ExamPointScore(ID);
        }

    }
    #endregion
}
