using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using Wisegar.Toolkit.Models.Email;

namespace Wisegar.Toolkit.Services.Email
{
    /// <summary>
    /// SMTP implementation of the generic email service
    /// </summary>
    public class EmailSmtpService : IEmailService
    {
        private readonly EmailSmtpSettings _emailSettings;
        private readonly ILogger<EmailSmtpService> _logger;

        public EmailSmtpService(IOptions<EmailSmtpSettings> emailSettings, ILogger<EmailSmtpService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Send a complex email using the EmailMessage template
        /// </summary>
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                if (emailMessage.To.Count == 0)
                {
                    _logger.LogWarning("There are no recipients specified in the EmailMessage");
                    return;
                }

                using var smtpClient = CreateSmtpClient();
                using var mailMessage = emailMessage.CreateComplexMailMessage();

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation("Complex email sent successfully. Subject: {subject}, Recipients: {count}", 
                    emailMessage.Subject, emailMessage.To.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending complex email. Subject: {subject}", emailMessage.Subject);
                throw;
            }
        }

        /// <summary>
        /// Create the configured SMTP client
        /// </summary>
        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };
        }
    }
}
