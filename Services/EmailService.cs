using System.Net;
using System.Net.Mail;
using System.Text;
using WebApplication2.Interfaces;

namespace WebApplication2.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Defina o e-mail do remetente e do destinatário
            MailMessage mailMessage = new MailMessage("ControlMoney@gmail.com", email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
            };

            // Configure o cliente SMTP
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.UseDefaultCredentials = false;


                smtpClient.Credentials = new NetworkCredential("marcosdev5411@gmail.com", "zfkh vdje cdpl twko");

                smtpClient.EnableSsl = true;

                // Enviar o e-mail
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
