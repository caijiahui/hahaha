using System;

namespace advt.Entity
{
    public partial class ExamSubject
    {

        public int ID { get; set; }

        public string SubjectName { get; set; }//������������
        public string TypeName { get; set; }//����ID
        public string ExamRuleName { get; set; }//���Թ���
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}