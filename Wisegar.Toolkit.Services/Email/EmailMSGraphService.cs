using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisegar.Toolkit.Services.Email
{
    public class EmailMSGraphService : IEmailService
    {
        private readonly ILogger<EmailMSGraphService> _logger;
        private readonly EmailSettings _emailSettings;
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var credential = new ClientSecretCredential(
                tenantId: "6cf36397-b654-4e66-bf51-61d16f203adc",
                clientId: "242cb218-3882-43dc-bcb2-a8c787077e99",
                clientSecret: "PhH8Q~D2jAzBu1fyJvF0cKUiU6SnU8fxXRKZOc.S"
            );

            var graphClient = new GraphServiceClient(credential, scopes);

            var message = new Message
            {
                Subject = "Hello from .NET and Microsoft Graph",
                Body = new ItemBody
                {
                    ContentType = BodyType.Text,
                    Content = "This is a test email sent using Microsoft Graph."
                },
                                ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = "yariel.re@gmail.com"
                            }
                        }
                    }
            };

            await graphClient.Users["info@wisegar.org"].SendMail.PostAsync(new SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            });

        }
    }
}
