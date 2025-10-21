using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Models.MSGraph;

namespace Wisegar.Toolkit.Services.xTest
{
    public static class SettingsService
    {
        public static IConfigurationRoot GetConfiguration()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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

    }
}
