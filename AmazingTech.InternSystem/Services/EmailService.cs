using System.Net.Mail;
using System.Net;
using AmazingTech.InternSystem.Models.DTO;

namespace AmazingTech.InternSystem.Services
{
    public interface IEmailService
    {
        public void SendMail(string Content, string ReceiveAddress, String Subject);
    }

    public class EmailService : IEmailService
    {

        public EmailService()
        {

        }

        public void SendMail(string Content,string ReceiveAddress, String Subject)
        {
            try
            {
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = Subject,
                    Body = Content,
                    IsBodyHtml = false,
                };
                mailMessage.From = new MailAddress(EmailSettingModel.Instance.FromEmailAddress, EmailSettingModel.Instance.FromDisplayName);
                mailMessage.To.Add(ReceiveAddress);

                var smtp = new SmtpClient()
                {
                    EnableSsl = EmailSettingModel.Instance.Smtp.EnableSsl,
                    Host = EmailSettingModel.Instance.Smtp.Host,
                    Port = EmailSettingModel.Instance.Smtp.Port,
                };
                var network = new NetworkCredential(EmailSettingModel.Instance.Smtp.EmailAddress, EmailSettingModel.Instance.Smtp.Password);
                smtp.Credentials = network;

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
