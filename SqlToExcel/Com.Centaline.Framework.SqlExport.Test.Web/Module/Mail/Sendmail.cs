using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SqlToExcel.Module.Mail
{
    public class Sendmail
    {

        public static string sendMail(string topic, string attachmentUrl, string body)
        {
            string sendAddress = "username@sina.com";//发件者邮箱地址
            string sendPassword = "password";//发件者邮箱密码
            string receiveAddress = "251469031@qq.com";//收件人收箱地址
            string mailTopic = topic;//主题
            string mailAttachment = attachmentUrl;//附件
            string mailBody = body;//内容
            string[] sendUsername = sendAddress.Split('@');

            SmtpClient client = new SmtpClient("smtp." + sendUsername[1].ToString()); //设置邮件协议

            client.UseDefaultCredentials = false;//这一句得写前面
            //client.EnableSsl = true;//服务器不支持SSL连接

            client.DeliveryMethod = SmtpDeliveryMethod.Network; //通过网络发送到Smtp服务器
            client.Credentials = new NetworkCredential(sendUsername[0].ToString(), sendPassword); //通过用户名和密码 认证
            MailMessage mmsg = new MailMessage(new MailAddress(sendAddress), new MailAddress(receiveAddress)); //发件人和收件人的邮箱地址
            mmsg.Subject = mailTopic;//邮件主题
            mmsg.SubjectEncoding = Encoding.UTF8;//主题编码
            mmsg.Body = mailBody;//邮件正文
            mmsg.BodyEncoding = Encoding.UTF8;//正文编码
            mmsg.IsBodyHtml = true;//设置为HTML格式 
            mmsg.Priority = MailPriority.High;//优先级
            if (mailAttachment.Trim() != "")
            {
                mmsg.Attachments.Add(new Attachment(mailAttachment));//增加附件
            }
            try
            {
                client.Send(mmsg);
                return null;
            }
            catch (Exception ee)
            {
                return ee.Message;
            }
        }





    }
}