using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace TeachMeBackendService.Logic
{
    public class EmailSmtpService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {       
            // adress and SMTP port of our Email server
            SmtpClient smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            // from mailbox
            string @from = ConfigurationManager.AppSettings["SmtpUserName"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];
            smtpClient.Credentials = new NetworkCredential(@from, password);

            // create Mail message
            var mail = new MailMessage(from, message.Destination)
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };

            return smtpClient.SendMailAsync(mail);
        }
    }






}