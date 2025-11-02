using Moq;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Services.Email;

namespace Wisegar.Toolkit.Services.xTest.Email
{
    public class EmailGApisServiceTest
    {
        private readonly IEmailService _emailService;

        public EmailGApisServiceTest()
        {
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<EmailGApisService>>();
            var emailSettingsMock = SettingsService.GetEmailGApiMockSettings();
            _emailService = new EmailGApisService(emailSettingsMock.Object, loggerMock.Object);
        }

        [Fact(Skip = "Google APIs service needs Service Account JSON configuration")]
        public async Task SendSimpleEmailTestAsync()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = "test@example.com",
                To = new List<string> { "recipient@example.com" },
                Subject = "Test Email via Google API",
                Body = "This is a test email sent via Google Gmail API",
                IsHtml = false
            };

            // Act & Assert
            try
            {
                await _emailService.SendEmailAsync(emailMessage);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception during email sending: {ex.Message}");
            }
        }
    }
}
