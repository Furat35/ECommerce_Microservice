using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Infrastructure;
using Ordering.Application.Models;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public async Task<bool> SendEmail(Email email)
        {
            //var client = new SendGridClient(_emailSettings.ApiKey);
            //var subject = email.Subject;
            //var to = new EmailAddress(email.To);
            //var emailBody = email.Body;
            //var from = new EmailAddress
            //{
            //    Email = _emailSettings.FromAddress,
            //    Name = _emailSettings.FromName
            //};
            //var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            //var response = await client.SendEmailAsync(sendGridMessage);

            _logger.LogInformation("Email sent.");
            return true;
            //if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
            //    return true;

            //_logger.LogError("Email sending failed.");
            //return false;
        }
    }
}
