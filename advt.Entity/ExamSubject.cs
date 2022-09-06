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
        public int ElectronicQuota { get; set; }//��λ����
        public bool IsSysPractice { get; set; }//ʵ���ɼ��Ƿ��Զ���ϵͳ����
        public bool IsProAssess { get; set; }//�Ƿ���Ƴ̿���
        public string  DepartCode { get; set; }
        public int MajorQuota { get; set; }//רҵ�Ӹ�
        public int SkillsAllowance { get; set; }//���ܽ���
        public int GradePosition { get; set; }//ְ�Ƚ���
        public bool IsAddAllowance { get; set; }//�����ۻ�����/�������Ӽ���
        public int PostQuota { get; set; }//��λ�ȼ�

    }
}