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
        public int HCLimit { get; set; }//HC����
        public bool IsSysPractice { get; set; }//ʵ���ɼ��Ƿ��Զ���ϵͳ����
        public bool IsProAssess { get; set; }//�Ƿ���Ƴ̿���
        public string  DepartCode { get; set; }

    }
}