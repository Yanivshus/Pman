using System;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Reflection;

namespace Pman
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            MailMessage massage = new MailMessage();
            massage.From = new MailAddress(this._emailSettings.SenderEmail);
            massage.Subject = subject;
            massage.To.Add(new MailAddress(recipientEmail));
            massage.Body = body;
            massage.IsBodyHtml = false;

            var smtpClient = new SmtpClient(_emailSettings.SmtpServer);
            smtpClient.Port = _emailSettings.SmtpPort;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);
            smtpClient.EnableSsl = true;
            smtpClient.Send(massage);
        }
    }
}
