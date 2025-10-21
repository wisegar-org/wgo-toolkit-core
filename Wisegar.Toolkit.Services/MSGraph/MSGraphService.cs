using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Wisegar.Toolkit.Models.MSGraph;

namespace Wisegar.Toolkit.Services.MSGraph
{
    public class MSGraphService
    {
        private readonly MSGraphSettings _msGraphSettings;

        public MSGraphService(IOptions<MSGraphSettings> msGraphSettings)
        {  
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
