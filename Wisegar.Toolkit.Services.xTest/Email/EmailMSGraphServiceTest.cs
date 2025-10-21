using Moq;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Services.Email;

namespace Wisegar.Toolkit.Services.xTest.Email
{
    public class EmailMSGraphServiceTest
    {
        private readonly IEmailService _emailService;
        
        public EmailMSGraphServiceTest()
        {
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<EmailMSGraphService>>();
            var emailSettingsMock = SettingsService.GetMSGraphMockSettings();   
            _emailService = new EmailMSGraphService(emailSettingsMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task SendSimpleEmailTestAsync() {
           
            var emailMessage = new EmailMessage
            {
                To = ["yariel.re@gmail.com"],
                Subject = "MS GRAPH - Monthly Report - CONFIDENTIAL",
                Body = @"
                    <html>
                    <body>
                        <h2 style='color: #2c5aa0;'>Monthly Report</h2>
                        <p>Dear,</p>
                        <p>Attached you will find the monthly report with the key indicators.</p>
                        <div style='background-color: #f0f8ff; padding: 10px; border-left: 4px solid #2c5aa0;'>
                            <strong>Executive Summary:</strong>
                            <ul>
                                <li>Sales: +15% vs previous month</li>
                                <li>New users: 1,250</li>
                                <li>Satisfaction: 4.8/5</li>
                            </ul>
                        </div>
                        <p>Kind regards,<br/>The analysis team</p>
                    </body>
                    </html>",
                IsHtml = true,
                Priority = EmailPriority.High,
                ReplyTo = "noreply@gmail.com",
                CustomHeaders = new Dictionary<string, string>
                {
                    { "X-Category", "Monthly-Report" },
                    { "X-Department", "Analytics" }
                }
            };

            try {
                await _emailService.SendEmailAsync(emailMessage);
            }
            catch (Exception ex) {
                Assert.Fail($"Exception during email sending: {ex.Message}");
            }            
        }
    }
}
