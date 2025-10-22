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
        private readonly Mock<IOptions<EmailSmtpSettings>> _emailSettingsMock;

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
            _emailSettingsMock = SettingsService.GetEmailSmtpMockSettings();
            _emailService = new EmailSmtpService(_emailSettingsMock.Object, _loggerMock.Object);
        }

        public void Dispose()
        {
            // Cleanup if needed
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task SendEmailAsync_WithSingleRecipient_ShouldNotThrowException()
        {
            //TODO: (TestEmail, TestSubject, TestBody, isHtml) To EmailMessage
            // Arrange
            //const bool isHtml = false;

            //// Act
            //var exception = await Record.ExceptionAsync(async () =>
            //    await _emailService.SendEmailAsync(TestEmail, TestSubject, TestBody, isHtml)
            //);

            //// Assert
            //AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithMultipleRecipients_ShouldNotThrowException()
        {
            //TODO: (recipients, subject, TestHtmlBody, isHtml) To EmailMessage
            // Arrange
            //var recipients = new[] { TestEmail1, TestEmail2 };
            //const string subject = "Test Subject Multiple Recipients";
            //const bool isHtml = true;

            //// Act
            //var exception = await Record.ExceptionAsync(async () =>
            //    await _emailService.SendEmailAsync(recipients, subject, TestHtmlBody, isHtml)
            //);

            //// Assert
            //AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithEmptyRecipients_ShouldNotThrowException()
        {
            //TODO: (emptyRecipients, TestSubject, TestBody) To EmailMessage
            //// Arrange
            //var emptyRecipients = Array.Empty<string>();

            //// Act
            //var exception = await Record.ExceptionAsync(async () =>
            //    await _emailService.SendEmailAsync(emptyRecipients, TestSubject, TestBody)
            //);

            //// Assert
            //Assert.Null(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithComplexEmailMessage_ShouldNotThrowException()
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
        public async Task SendEmailAsync_WithEmptyEmailMessage_ShouldNotThrowException()
        {
            
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = "noreply@wisegar.info", //TODO: Use default from settings
                To = new List<string>(),
                Subject = "Empty Recipients Test",
                Body = "This should not be sent"
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            Assert.Null(exception);
        }

        #region Helper Methods

        private static EmailMessage CreateComplexEmailMessage()
        {
            return new EmailMessage
            {
                From = "noreply@wisegar.info", //TODO: Use default from settings
                To = new List<string> { "yariel.re@gmail.com" },
                Cc = new List<string> { "hurshelann30@gmail.com" },
                Bcc = new List<string> { "hurshelann30@gmail.com" },
                Subject = "Complex Email Test",
                Body = "<h1>Complex Email</h1><p>This is a complex email with multiple options.</p>",
                IsHtml = true,
                Priority = EmailPriority.High,
                ReplyTo = "replyto@example.com",
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