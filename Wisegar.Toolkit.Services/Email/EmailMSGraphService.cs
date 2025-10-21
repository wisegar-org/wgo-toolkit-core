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

            var message = new Message
            {
                Subject = emailMessage.Subject,
                Body = new ItemBody
                {
                    ContentType = emailMessage.IsHtml ? BodyType.Html : BodyType.Text,
                    Content = emailMessage.Body,
                },
            };

            // To Recipients
            foreach (var to in emailMessage.To)
            {
                message.ToRecipients ??= [];
                message.ToRecipients.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = to
                    }
                });
            }

            // CC Recipients
            foreach (var cc in emailMessage.Cc)
            {
                message.CcRecipients ??= [];
                message.CcRecipients.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = cc
                    }
                });
            }

            // BCC Recipients
            foreach (var bcc in emailMessage.Bcc)
            {
                message.BccRecipients ??= [];
                message.BccRecipients.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = bcc
                    }
                });
            }

            // Attachments
            foreach (var attachment in emailMessage.Attachments)
            {
                if (attachment.Content.Length > 0)
                {
                    message.Attachments ??= [];
                    var fileAttachment = new FileAttachment
                    {
                        OdataType = "#microsoft.graph.fileAttachment",
                        Name = attachment.FileName,
                        ContentType = attachment.ContentType,
                        ContentBytes = attachment.Content
                    };
                    message.Attachments.Add(fileAttachment);
                }
            }

            // Set Priority
            switch (emailMessage.Priority)
            {
                case EmailPriority.High:
                    message.Importance = Importance.High;
                    break;
                case EmailPriority.Low:
                    message.Importance = Importance.Low;
                    break;
                case EmailPriority.Normal:
                    break;
                default:
                    message.Importance = Importance.Normal;
                    break;
            }

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
