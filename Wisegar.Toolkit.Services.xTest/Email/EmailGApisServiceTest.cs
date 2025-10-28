using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Services.Email;
using Wisegar.Toolkit.Services.GApis;

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

        [Fact]
        public async Task SendSimpleEmailTestAsync()
        {
         

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
