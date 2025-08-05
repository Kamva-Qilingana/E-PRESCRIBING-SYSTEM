using System.Net;
using System.Net.Mail;

namespace E_PRESCRIBING_SYSTEM.EmailerSender
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message, bool isHtml = true)
        {
            var mail = "baybreezeday@gmail.com";
            var pw = "yscu uxlt afal fefg";

            var client = new SmtpClient("smpt.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml
            };

            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        } 
    }
}
