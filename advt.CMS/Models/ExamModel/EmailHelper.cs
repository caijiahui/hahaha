using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class EmailHelper
    {       
        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="mailTo">接收人邮件</param>
        /// <param name="mailTitle">发送邮件标题</param>
        /// <param name="mailContent">发送邮件内容</param>
        /// <returns></returns>
        public string SendEmail()
        {
            var result = "";
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add("yin.chen@advantech.com.cn");//发送
            msg.CC.Add("yin.chen@advantech.com.cn");//抄送           
            msg.From = new MailAddress("HREmail", "yinchen", System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.Subject = "考试人员";//邮件标题
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码
            msg.Body = "以下是考试成绩查询";//邮件内容
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码
            msg.IsBodyHtml = false;//是否是HTML邮件
            msg.Priority = MailPriority.High;//邮件优先级
            SmtpClient client = new SmtpClient();
     
            client.Port = 587;//Gmail使用的端口
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;//经过ssl加密
            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);       
                result += "发送成功";
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                result += "发送邮件出错";
            }
            return result;
        }
        
    }
}