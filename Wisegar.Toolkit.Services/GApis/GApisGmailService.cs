using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Options;
using Wisegar.Toolkit.Models.GApis;

namespace Wisegar.Toolkit.Services.GApis
{
    public class GApisGmailService
    {
        private readonly GApisSettings _gapisSettings;

        public GApisGmailService(IOptions<GApisSettings> gapisSettings)
        {
            _gapisSettings = gapisSettings.Value;
        }

        [Obsolete]
        public GmailService GetServiceConnection()
        {
            var userToImpersonate = _gapisSettings.UserName;
            var serviceAccountJsonPath = _gapisSettings.JsonPath;

            string[] scopes = { GmailService.Scope.GmailSend };


            using var stream = File.OpenRead(path);
            var json = await JsonDocument.ParseAsync(stream);


            GoogleCredential credential;

            using (var stream = new FileStream(serviceAccountJsonPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                                             .CreateScoped(scopes)
                                             .CreateWithUser(userToImpersonate);
            }

            return new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Wisegar Gmail API Client"
            });

        }
    }
}
