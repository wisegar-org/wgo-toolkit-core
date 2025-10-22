using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Wisegar.Toolkit.Models.Email;

namespace Wisegar.Toolkit.Services.Email
{
    public static class EmailExtensions
    {
        /// <summary>
        /// Convert custom priority to MailPriority
        /// </summary>
        public static MailPriority ConvertPriority(this EmailPriority priority)
        {
            return priority switch
            {
                EmailPriority.Low => MailPriority.Low,
                EmailPriority.High => MailPriority.High,
                _ => MailPriority.Normal
            };
        }

        /// <summary>
        /// Create a complex email message with all the options
        /// </summary>
        public static MailMessage CreateComplexMailMessage(this EmailMessage emailMessage)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailMessage.From),
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = emailMessage.IsHtml,
                Priority = ConvertPriority(emailMessage.Priority)
            };

            // Add primary recipients
            foreach (var to in emailMessage.To.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                mailMessage.To.Add(to.Trim());
            }

            // Add CC recipients
            foreach (var cc in emailMessage.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                mailMessage.CC.Add(cc.Trim());
            }

            // Add blind carbon copy (BCC) recipients
            foreach (var bcc in emailMessage.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                mailMessage.Bcc.Add(bcc.Trim());
            }

            // Set Reply-To if specified
            if (!string.IsNullOrWhiteSpace(emailMessage.ReplyTo))
            {
                mailMessage.ReplyToList.Add(emailMessage.ReplyTo);
            }

            // Add custom headers
            foreach (var header in emailMessage.CustomHeaders)
            {
                mailMessage.Headers.Add(header.Key, header.Value);
            }

            // Add attachments
            foreach (var attachment in emailMessage.Attachments)
            {
                if (attachment.Content.Length > 0)
                {
                    var stream = new MemoryStream(attachment.Content);
                    var mailAttachment = new System.Net.Mail.Attachment(stream, attachment.FileName, attachment.ContentType);
                    mailMessage.Attachments.Add(mailAttachment);
                }
            }

            return mailMessage;
        }


        public static Message ToMSGraphMessage(this EmailMessage emailMessage) {
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

            return message;
        }

        /// <summary>
        /// Create the email message //TODO: Refactor to use EmailMessage
        /// </summary>
        public static MailMessage CreateMailMessage(string from, string[] to, string subject, string body, bool isHtml)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            foreach (var toEmail in to)
            {
                if (!string.IsNullOrWhiteSpace(toEmail))
                {
                    mailMessage.To.Add(toEmail.Trim());
                }
            }

            return mailMessage;
        }

    }
}
