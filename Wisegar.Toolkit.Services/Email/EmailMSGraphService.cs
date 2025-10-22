using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Models.MSGraph;
using Wisegar.Toolkit.Services.MSGraph;

namespace Wisegar.Toolkit.Services.Email
{
    public class EmailMSGraphService : IEmailService
    {
        private readonly ILogger<EmailMSGraphService> _logger;
        private readonly MSGraphSettings _msGraphEmailSettings;
        private readonly MSGraphService _msGraphService;
        public EmailMSGraphService(IOptions<MSGraphSettings> msGraphEmailSettings, ILogger<EmailMSGraphService> logger)
        {
            _msGraphEmailSettings = msGraphEmailSettings.Value;
            _msGraphService = new MSGraphService(msGraphEmailSettings);
            _logger = logger;    
        }
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var graphClient = _msGraphService.GetClientService();

           var message = emailMessage.ToMSGraphMessage();

            // Reply-To, Custom Headers are not directly supported in this version of Microsoft Graph SDK 
            var request = new SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            };

            await graphClient.Users[_msGraphEmailSettings.Principal].SendMail.PostAsync(request);
        }
    }
}
