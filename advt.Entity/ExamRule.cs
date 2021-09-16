using System;

namespace advt.Entity
{
    public partial class ExamRule
    {

        #region ExamRule , (Ver:2.3.8) at: 2021/1/9 12:16:03

        public int ID { get; set; }
        public string RuleName { get; set; }//������
        public string TypeName { get; set; }//��������
        public string SubjectName { get; set; }//���Կ�Ŀ

        public decimal TotalScore { get; set; }//�ܷ�

        public decimal TotalTime { get; set; }//��ʱ��

        public int TotalSubject { get; set; }//����

        public decimal PassScore { get; set; }//ͨ������

        public bool IsRead { get; set; }

        public bool IsRepeat { get; set; }

        public string StartDeac { get; set; }

        public string EndDesc { get; set; }

        public string CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool IsQuestion { get; set; }
        public decimal PassPracticeScore { get; set; }
        #endregion
    }
}