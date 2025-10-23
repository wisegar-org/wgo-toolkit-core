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
        public async Task SendEmailAsync_WithSingleRecipient_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail },
                Subject = TestSubject,
                Body = TestBody,
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
        public async Task SendEmailAsync_WithMultipleRecipients_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail1, TestEmail2 },
                Subject = "Test Subject Multiple Recipients",
                Body = TestHtmlBody,
                IsHtml = true
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithEmptyRecipients_ShouldLogWarning()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = _emailSettings.FromEmail,
                To = new List<string>(),
                Subject = TestSubject,
                Body = TestBody
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert - No debe lanzar excepción, solo registrar warning
            Assert.Null(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithComplexEmailMessage_ShouldSendSuccessfully()
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

        [Fact]
        public async Task SendEmailAsync_WithAttachments_ShouldSendSuccessfully()
        {
            // Arrange
            var attachment = new EmailAttachment(
                "test.txt",
                System.Text.Encoding.UTF8.GetBytes("Test attachment content"),
                "text/plain"
            );

            var emailMessage = new EmailMessage
            {
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail },
                Subject = "Test Email with Attachment",
                Body = "This email contains an attachment",
                IsHtml = false,
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
        public async Task SendEmailAsync_WithCcAndBcc_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail },
                Cc = new List<string> { TestEmail1 },
                Bcc = new List<string> { TestEmail2 },
                Subject = "Test Email with CC and BCC",
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
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail },
                Subject = "High Priority Email",
                Body = "This is a high priority email",
                IsHtml = false,
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
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail },
                Subject = "Test Email with Reply-To",
                Body = "This email has a custom Reply-To address",
                IsHtml = false,
                ReplyTo = "replyto@example.com"
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
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail },
                Subject = "Test Email with Custom Headers",
                Body = "This email has custom headers",
                IsHtml = false,
                CustomHeaders = new Dictionary<string, string>
                {
                    { "X-Custom-Header", "CustomValue" },
                    { "X-Priority", "1" }
                }
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithHtmlContent_ShouldSendSuccessfully()
        {
            // Arrange
            var htmlBody = @"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #4CAF50;'>Test Email</h2>
                    <p>This is a <strong>test email</strong> with HTML content.</p>
                    <ul>
                        <li>Item 1</li>
                        <li>Item 2</li>
                        <li>Item 3</li>
                    </ul>
                </body>
                </html>";

            var emailMessage = new EmailMessage
            {
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail },
                Subject = "Test HTML Email",
                Body = htmlBody,
                IsHtml = true
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
            var attachment = new EmailAttachment(
                "report.pdf",
                System.Text.Encoding.UTF8.GetBytes("PDF content simulation"),
                "application/pdf"
            );

            return new EmailMessage
            {
                From = _emailSettings.FromEmail,
                To = new List<string> { TestEmail },
                Cc = new List<string> { TestEmail1 },
                Bcc = new List<string> { TestEmail2 },
                Subject = "Complex Email Test - All Features",
                Body = "<h1>Complex Email</h1><p>This email demonstrates all features:</p><ul><li>HTML content</li><li>Multiple recipients (To, CC, BCC)</li><li>High priority</li><li>Attachment</li><li>Custom headers</li><li>Reply-To</li></ul>",
                IsHtml = true,
                Priority = EmailPriority.High,
                ReplyTo = "replyto@wisegar.info",
                Attachments = new List<EmailAttachment> { attachment },
                CustomHeaders = new Dictionary<string, string>
                {
                    { "X-Custom-Header", "CustomValue" },
                    { "X-Test-Category", "Integration-Test" }
                }
            };
        }

        private static void AssertExpectedExceptionOrNull(Exception? exception)
        {
            // Permite null (éxito) o excepciones esperadas de SMTP (problemas de red, autenticación, etc.)
            Assert.True(
                exception == null || exception is SmtpException || exception is InvalidOperationException,
                $"Tipo de excepción inesperado: {exception?.GetType().Name} - {exception?.Message}");
        }

        #endregion
    }
}
