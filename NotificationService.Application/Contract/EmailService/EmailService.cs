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
using MailKit.Security;
using NotificationService.Domain.Enums;
using System.Net.Http;
using System.Net.Http.Json;

namespace NotificationService.Application.Contract.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly mailSettings _mailSettings;
        private readonly IMongoDbLogRepository _mongodb;
        private readonly HttpClient _httpClient;
        public EmailService(mailSettings mailSettings, IMongoDbLogRepository mongodb, HttpClient httpClient)
        {
            _mailSettings = mailSettings ?? throw new ArgumentNullException(nameof(mailSettings));
            _mongodb = mongodb;
            _httpClient = httpClient;
        }
        public async Task SendEmailAsync(EmailRequest request)
        {
            var emailMessage = CreateEmailMessage(request);
            Send(emailMessage);


            var notificationActivity = new NotificationActivity()
            {
                SentTo = request.To,
                SentAt = DateTime.Now,
                Status = true,
                Purpose = request.Subject,
                NotificationType = NotificationType.Email,
                HasAttachment = true,

            };
            await _mongodb.CreateLog(notificationActivity);
            var registrationRequest = new RegistrationRequest()
            {
                Email = request.To,
                // Map other properties as needed
            };
            var response = await _httpClient.PostAsJsonAsync("https://registration-service.com/api/register", registrationRequest);
            response.EnsureSuccessStatusCode();
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
