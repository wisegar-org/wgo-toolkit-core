using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Mail;
using Wisegar.Toolkit.Services.Email;

namespace Wisegar.Toolkit.Services.xTest.Email
{
    public class EmailSmtpServiceTest
    {
        private readonly Mock<IOptions<EmailSettings>> _emailSettingsMock;
        private readonly Mock<ILogger<EmailSmtpService>> _loggerMock;
        private readonly Mock<SmtpClient> _smtpClientMock;

        public EmailSmtpServiceTest()
        {
            //TODO: Extract from appSettings file
            var emailSettings = new EmailSettings
            {
                SmtpServer = "",
                SmtpPort = 0,
                EnableSsl = false,
                Username = "",
                Password = "",
                FromEmail = "",
                FromName = "",  
                ToEmails = new List<string> { },
                Subject = "Test Email"
            };
            var optionsEmailSettings = Options.Create(emailSettings);
            _emailSettingsMock = new Mock<IOptions<EmailSettings>>();
            _emailSettingsMock.Setup(es => es.Value).Returns(optionsEmailSettings.Value);
            _loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<EmailSmtpService>>();
            _smtpClientMock = new Mock<SmtpClient>();
        }

        [Fact]
        public async Task SendEmailAsync_ShouldSendEmail()
        {
            // Arrange
            var emailService = new EmailSmtpService(_emailSettingsMock.Object, _loggerMock.Object);
            //Act
            await emailService.SendEmailAsync(["yariel.re@gmail.com"], "", "");
            // Assert
            _smtpClientMock.Verify(s => s.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Email sent successfully")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}