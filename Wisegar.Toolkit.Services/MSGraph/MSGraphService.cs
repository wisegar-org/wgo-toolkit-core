using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wisegar.Toolkit.Services.Email;

namespace Wisegar.Toolkit.Services.MSGraph
{
    public class MSGraphService
    {
        private readonly ILogger<MSGraphService> _logger;
        private readonly MSGraphSettings _msGraphSettings;

        public MSGraphService(IOptions<MSGraphSettings> msGraphSettings, ILogger<MSGraphService> logger)
        {
            _logger = logger;   
            _msGraphSettings = msGraphSettings.Value;
        }
        public GraphServiceClient GetClientService()
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var credential = new ClientSecretCredential(
                tenantId: _msGraphSettings.TenantId,
                clientId: _msGraphSettings.ClientId,
                clientSecret: _msGraphSettings.ClientSecret
            );

            return new GraphServiceClient(credential, scopes);
        }
    }
}
