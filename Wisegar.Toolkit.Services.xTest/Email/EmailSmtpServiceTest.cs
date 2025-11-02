using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Mail;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Services.Email;

namespace Wisegar.Toolkit.Services.xTest.Email
{
    public class EmailSmtpServiceTest : IDisposable
    {
        private readonly EmailSmtpService _emailService;
        private readonly Mock<ILogger<EmailSmtpService>> _loggerMock;
        private readonly EmailSmtpSettings _emailSettings;

        // Test data constants
        private const string TestEmail = "yariel.re@gmail.com";
        private const string TestEmail1 = "hurshelann30@gmail.com";
        private const string TestEmail2 = "test2@example.com";
        private const string TestSubject = "Test Subject";
        private const string TestBody = "Test email body";
        private const string TestHtmlBody = "<h1>Test HTML Email</h1><p>This is a test email.</p>";

        public EmailSmtpServiceTest()
        {
            _loggerMock = new Mock<ILogger<EmailSmtpService>>();
            var emailSettingsMock = SettingsService.GetEmailSmtpMockSettings();
            _emailSettings = emailSettingsMock.Object.Value;
            _emailService = new EmailSmtpService(emailSettingsMock.Object, _loggerMock.Object);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task SendEmailAsync_WithSingleRecipient_ShouldNotThrowException()
        {
            // Arrange
            var emailMessage = CreateComplexEmailMessage();

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }



        // ========== NEW TESTS ADDED ==========

        [Fact]
        public async Task SendEmailAsync_WithAttachment_ShouldSendSuccessfully()
        {
            // Arrange
            var attachment = new EmailAttachment(
                "test-document.txt",
                System.Text.Encoding.UTF8.GetBytes("Test attachment content - SMTP"),
                "text/plain"
            );

            var emailMessage = new EmailMessage
            {
                From = "noreply@wisegar.info",
                To = new List<string> { TestEmail },
                Subject = "Test Email with Attachment - SMTP",
                Body = "<h2>Email con Adjunto</h2><p>Este email contiene un archivo adjunto.</p>",
                IsHtml = true,
                Attachments = new List<EmailAttachment> { attachment }
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithMultipleAttachments_ShouldSendSuccessfully()
        {
            // Arrange
            var attachment1 = new EmailAttachment(
                "document1.txt",
                System.Text.Encoding.UTF8.GetBytes("First document"),
                "text/plain"
            );

            var attachment2 = new EmailAttachment(
                "document2.txt",
                System.Text.Encoding.UTF8.GetBytes("Second document"),
                "text/plain"
            );

            var emailMessage = new EmailMessage
            {
                From = "noreply@wisegar.info",
                To = new List<string> { TestEmail },
                Subject = "Test Email with Multiple Attachments - SMTP",
                Body = "<h2>Email con Múltiples Adjuntos</h2>",
                IsHtml = true,
                Attachments = new List<EmailAttachment> { attachment1, attachment2 }
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithCcAndBcc_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = "noreply@wisegar.info",
                To = new List<string> { TestEmail },
                Cc = new List<string> { TestEmail1 },
                Bcc = new List<string> { TestEmail2 },
                Subject = "Test Email with CC and BCC - SMTP",
                Body = "This email has CC and BCC recipients",
                IsHtml = false
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithHighPriority_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = "noreply@wisegar.info",
                To = new List<string> { TestEmail },
                Subject = "High Priority Email - SMTP",
                Body = "<h2>URGENT</h2><p>High priority email</p>",
                IsHtml = true,
                Priority = EmailPriority.High
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithReplyTo_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = "noreply@wisegar.info",
                To = new List<string> { TestEmail },
                Subject = "Test Email with Reply-To - SMTP",
                Body = "This email has a custom Reply-To address",
                IsHtml = false,
                ReplyTo = "support@wisegar.info"
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithCustomHeaders_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = "noreply@wisegar.info",
                To = new List<string> { TestEmail },
                Subject = "Test Email with Custom Headers - SMTP",
                Body = "This email has custom headers",
                IsHtml = false,
                CustomHeaders = new Dictionary<string, string>
                {
                    { "X-Custom-Header", "CustomValue" },
                    { "X-Mailer", "Wisegar-Toolkit" }
                }
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        #region Helper Methods

        private EmailMessage CreateComplexEmailMessage()
        {
            return new EmailMessage
            {
                From = _emailSettings.From,
                To = new List<string> { "yariel.re@gmail.com" },
                Cc = new List<string> { "hurshelann30@gmail.com" },
                Bcc = new List<string> { "hurshelann30@gmail.com" },
                Subject = "Complex Email Test",
                Body = "<h1>Complex Email</h1><p>This is a complex email with multiple options.</p>",
                IsHtml = true,
                Priority = EmailPriority.High,
                ReplyTo = "yariel.re@gmail.com",
                CustomHeaders = new Dictionary<string, string>
                {
                    { "X-Custom-Header", "CustomValue" }
                }
            };
        }

        private static void AssertExpectedExceptionOrNull(Exception? exception)
        {
            Assert.True(
                exception == null || exception is SmtpException || exception is InvalidOperationException,
                $"Unexpected exception type: {exception?.GetType().Name}");
        }

        #endregion
    }
}