using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using NotificationService.Application.Contract.IEmailContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Contract.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly mailSettings _mailSettings;

        public EmailService(mailSettings mailSettings)
        {
            _mailSettings = mailSettings ?? throw new ArgumentNullException(nameof(mailSettings));
        }
        public void SendEmail(EmailRequest request)
        {
            var emailMessage = CreateEmailMessage(request);
            Send(emailMessage);
        }



        private MimeMessage CreateEmailMessage(EmailRequest request)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_mailSettings.From));
            emailMessage.To.Add(MailboxAddress.Parse(request.To));
            emailMessage.Subject = request.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.SmtpServer, _mailSettings.Port, true);
            smtp.Authenticate(_mailSettings.Username, _mailSettings.Password);
            smtp.Send(mailMessage);
            smtp.Disconnect(true);
        }
    }
}
