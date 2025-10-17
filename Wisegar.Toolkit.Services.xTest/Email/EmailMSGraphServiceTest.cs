using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Mail;
using Wisegar.Toolkit.Services.Email;
using Wisegar.Toolkit.Services.MSGraph;

namespace Wisegar.Toolkit.Services.xTest.Email
{
    public class EmailMSGraphServiceTest
    {
        private readonly Mock<IOptions<MSGraphSettings>> _emailSettingsMock;
        private readonly Mock<ILogger<MSGraphService>> _loggerMock;

        public EmailMSGraphServiceTest()
        {
            //TODO: Add proper settings for MSGraph
        }
    }
}
