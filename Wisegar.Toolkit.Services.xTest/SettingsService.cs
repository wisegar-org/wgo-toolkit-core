using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Models.GApis;
using Wisegar.Toolkit.Models.MSGraph;

namespace Wisegar.Toolkit.Services.xTest
{
    public static class SettingsService
    {
        public static IConfigurationRoot GetConfiguration()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
            .Build();
            return config;
        }

        public static Mock<IOptions<MSGraphSettings>> GetMSGraphMockSettings()
        {
            var config = SettingsService.GetConfiguration();
            var msGraphSettingsSection = config.GetSection(MSGraphSettings.SectionName) ?? throw new ArgumentNullException("MSGraph settings section not found in appsettings.json");
            var msGraphSettings = msGraphSettingsSection.Get<MSGraphSettings>() ?? throw new ArgumentNullException("MSGraph settings could not be bound from configuration section");
            var emailSettingsMock = new Mock<IOptions<MSGraphSettings>>();
            emailSettingsMock.Setup(es => es.Value).Returns(msGraphSettings);
            return emailSettingsMock;
        }

        public static Mock<IOptions<EmailSmtpSettings>> GetEmailSmtpMockSettings()
        {
            var config = SettingsService.GetConfiguration();
            var smtpSettingsSection = config.GetSection(EmailSmtpSettings.SectionName) ?? throw new ArgumentNullException("Smtp settings section not found in appsettings.json");
            var smtpSettings = smtpSettingsSection.Get<EmailSmtpSettings>() ?? throw new ArgumentNullException("Smtp settings could not be bound from configuration section");
            var emailSettingsMock = new Mock<IOptions<EmailSmtpSettings>>();
            emailSettingsMock.Setup(es => es.Value).Returns(smtpSettings);
            return emailSettingsMock;
        }

        public static Mock<IOptions<GApisSettings>> GetEmailGApiMockSettings()
        {
            var config = SettingsService.GetConfiguration();
            var gapisSettingsSection = config.GetSection(GApisSettings.SectionName) ?? throw new ArgumentNullException("GApis settings section not found in appsettings.json");
            var gapisSettings = gapisSettingsSection.Get<GApisSettings>() ?? throw new ArgumentNullException("GApis settings could not be bound from configuration section");
            var gapisSettingsMock = new Mock<IOptions<GApisSettings>>();
            gapisSettingsMock.Setup(es => es.Value).Returns(gapisSettings);
            return gapisSettingsMock;
        }

        public static EmailMessage CreateComplexEmailMessage()
        {
            return new EmailMessage
            {
                From = "yariel.re@gmail.com",
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
                ReplyTo = "yariel.re@gmail.com",
                CustomHeaders = new Dictionary<string, string>
                {
                    { "X-Category", "Monthly-Report" },
                    { "X-Department", "Analytics" }
                }
            };
        }

    }
}
