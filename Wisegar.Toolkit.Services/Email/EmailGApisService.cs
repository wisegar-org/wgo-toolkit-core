using Google.Apis.Gmail.v1.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Models.GApis;
using Wisegar.Toolkit.Services.GApis;

namespace Wisegar.Toolkit.Services.Email
{
    public class EmailGApisService : IEmailService
    {
        private readonly ILogger<EmailGApisService> _logger;
        private readonly GApisSettings _gapisSettings;
        private readonly GApisGmailService _gapisGmailService;

        public EmailGApisService(IOptions<GApisSettings> gapisSettings, ILogger<EmailGApisService> logger) {
            _gapisSettings = gapisSettings.Value;
            _gapisGmailService = new GApisGmailService(gapisSettings);
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var gmailService = _gapisGmailService.GetServiceConnection();
            var mailMessage = new MailMessage(_gapisSettings.UserName, emailMessage.To[0],emailMessage.Subject, emailMessage.Body);
            var mimeMessage = ConvertToMimeMessage(mailMessage);

            var gmailMessage = new Message
            {
                Raw = mimeMessage
            };

            try {
                var sendRequest = gmailService.Users.Messages.Send(gmailMessage, _gapisSettings.UserName);
                await sendRequest.ExecuteAsync();
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        private string ConvertToMimeMessage(MailMessage message)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.WriteLine($"To: {message.To}");
                writer.WriteLine($"From: {message.From}");
                writer.WriteLine($"Subject: {message.Subject}");
                writer.WriteLine("Content-Type: text/plain; charset=utf-8");
                writer.WriteLine();
                writer.WriteLine(message.Body);
                writer.Flush();

                var bytes = stream.ToArray();
                return Convert.ToBase64String(bytes)
                              .Replace('+', '-')
                              .Replace('/', '_')
                              .Replace("=", "");
            }
        }

    }
}
