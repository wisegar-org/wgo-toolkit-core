using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Mail;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Services.Email;

namespace Wisegar.Toolkit.Services.xTest.Email
{
    public class EmailSmtpServiceTest
    {
        private readonly IEmailService emailService;

        public EmailSmtpServiceTest()
        {
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<EmailSmtpService>>();
            var emailSettingsMock = SettingsService.GetEmailSmtpMockSettings();
            emailService = new EmailSmtpService(emailSettingsMock.Object, loggerMock.Object);
        }

        [Fact]
        public void SendEmailTest()
        {
            //TODO: Implement actual test
            try
            {
                // Arrange
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception during email sending: {ex.Message}");
            }

        }
    }
}