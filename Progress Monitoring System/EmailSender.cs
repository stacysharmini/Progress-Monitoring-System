using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Progress_Monitoring_System
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            string fromMail = "stacysharmini@gmail.com";  
            string fromPassword = "gekpkvfozyrprhvg"; 

            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
               
                smtpClient.Port = 587; 
                smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                smtpClient.EnableSsl = true; 

              
                var message = new MailMessage(fromMail, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

           
                try
                {
                    await smtpClient.SendMailAsync(message);
                }
                catch (SmtpException ex)
                {
                   
                    throw new InvalidOperationException("Error sending email.", ex);
                }
            }
        }
    }
}
