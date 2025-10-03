using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Wisegar.Toolkit.Services.Email
{
    /// <summary>
    /// SMTP implementation of the generic email service
    /// </summary>
    public class EmailSmtpService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSmtpService> _logger;

        public EmailSmtpService(IOptions<EmailSettings> emailSettings, ILogger<EmailSmtpService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Send a generic email to multiple recipients
        /// </summary>
        public async Task SendEmailAsync(string[] to, string subject, string body, bool isHtml = false)
        {
            try
            {
                if (to == null || !to.Any())
                {
                    _logger.LogWarning("There are no recipients specified to send the email to.");
                    return;
                }

                using var smtpClient = CreateSmtpClient();
                using var mailMessage = CreateMailMessage(to, subject, body, isHtml);

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation("Email sent successfully to {count} recipients. Subject: {subject}", 
                    to.Length, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email. Subject: {subject}", subject);
                throw;
            }
        }

        /// <summary>
        /// Send a generic email to a single recipient
        /// </summary>
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            await SendEmailAsync(new[] { to }, subject, body, isHtml);
        }

        /// <summary>
        /// Send a complex email using the EmailMessage template
        /// </summary>
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                if (!emailMessage.To.Any())
                {
                    _logger.LogWarning("There are no recipients specified in the EmailMessage");
                    return;
                }

                using var smtpClient = CreateSmtpClient();
                using var mailMessage = CreateComplexMailMessage(emailMessage);

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
        /// Sends a specific error notification for website monitoring
        /// </summary>
        public async Task SendErrorNotificationAsync(string websiteUrl, int statusCode, string status, string? errorMessage = null)
        {
            try
            {
                if (!_emailSettings.ToEmails.Any())
                {
                    _logger.LogWarning("There are no email addresses configured to send error notifications.");
                    return;
                }

                var subject = $"{_emailSettings.Subject} - {websiteUrl}";
                var body = BuildErrorNotificationBody(websiteUrl, statusCode, status, errorMessage);

                await SendEmailAsync(_emailSettings.ToEmails.ToArray(), subject, body, isHtml: true);

                _logger.LogInformation("Error notification sent for {url}", websiteUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending error notification for {url}", websiteUrl);
            }
        }

        /// <summary>
        /// Create the configured SMTP client
        /// </summary>
        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };
        }

        /// <summary>
        /// Create the email message
        /// </summary>
        private MailMessage CreateMailMessage(string[] to, string subject, string body, bool isHtml)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            foreach (var toEmail in to)
            {
                if (!string.IsNullOrWhiteSpace(toEmail))
                {
                    mailMessage.To.Add(toEmail.Trim());
                }
            }

            return mailMessage;
        }

        /// <summary>
        /// Create a complex email message with all the options
        /// </summary>
        private MailMessage CreateComplexMailMessage(EmailMessage emailMessage)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = emailMessage.IsHtml,
                Priority = ConvertPriority(emailMessage.Priority)
            };

            // Add primary recipients
            foreach (var to in emailMessage.To.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                mailMessage.To.Add(to.Trim());
            }

            // Add CC recipients
            foreach (var cc in emailMessage.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                mailMessage.CC.Add(cc.Trim());
            }

            // Add blind carbon copy (BCC) recipients
            foreach (var bcc in emailMessage.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                mailMessage.Bcc.Add(bcc.Trim());
            }

            // Set Reply-To if specified
            if (!string.IsNullOrWhiteSpace(emailMessage.ReplyTo))
            {
                mailMessage.ReplyToList.Add(emailMessage.ReplyTo);
            }

            // Add custom headers
            foreach (var header in emailMessage.CustomHeaders)
            {
                mailMessage.Headers.Add(header.Key, header.Value);
            }

            // Add attachments
            foreach (var attachment in emailMessage.Attachments)
            {
                if (attachment.Content.Length > 0)
                {
                    var stream = new MemoryStream(attachment.Content);
                    var mailAttachment = new Attachment(stream, attachment.FileName, attachment.ContentType);
                    mailMessage.Attachments.Add(mailAttachment);
                }
            }

            return mailMessage;
        }

        /// <summary>
        /// Convert custom priority to MailPriority
        /// </summary>
        private MailPriority ConvertPriority(EmailPriority priority)
        {
            return priority switch
            {
                EmailPriority.Low => MailPriority.Low,
                EmailPriority.High => MailPriority.High,
                _ => MailPriority.Normal
            };
        }

        /// <summary>
        /// Build the HTML body for monitoring error notifications
        /// </summary>
        private string BuildErrorNotificationBody(string websiteUrl, int statusCode, string status, string? errorMessage)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var body = $@"
        <html>
        <body>
            <h2 style='color: #d32f2f;'>🚨 ALERT: Website Down</h2>
            
            <div style='background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 10px 0;'>
                <p><strong>Website:</strong> <a href='{websiteUrl}'>{websiteUrl}</a></p>
                <p><strong>Status Code:</strong> {statusCode}</p>
                <p><strong>Status:</strong> <span style='color: #d32f2f; font-weight: bold;'>{status}</span></p>
                <p><strong>Date and Time:</strong> {timestamp}</p>
                {(errorMessage != null ? $"<p><strong>Error:</strong> {errorMessage}</p>" : "")}
            </div>
            
            <hr>
            <p style='font-size: 12px; color: #666;'>
                This message was automatically generated by the Website Monitor Service.
            </p>
        </body>
        </html>";

            return body;
        }
    }
}
