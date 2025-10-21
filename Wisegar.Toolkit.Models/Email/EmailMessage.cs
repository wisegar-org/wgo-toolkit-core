using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisegar.Toolkit.Models.Email
{
    /// <summary>
    /// Template for representing a complex email message with multiple options
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Main email recipients
        /// </summary>
        public List<string> To { get; set; } = new();

        /// <summary>
        /// Copy recipients (CC)
        /// </summary>
        public List<string> Cc { get; set; } = new();

        /// <summary>
        /// Blind carbon copy (BCC) recipients
        /// </summary>
        public List<string> Bcc { get; set; } = new();

        /// <summary>
        /// Email subject
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Body of the email
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the body is HTML
        /// </summary>
        public bool IsHtml { get; set; } = false;

        /// <summary>
        /// Email priority
        /// </summary>
        public EmailPriority Priority { get; set; } = EmailPriority.Normal;

        /// <summary>
        /// Attachments
        /// </summary>
        public List<EmailAttachment> Attachments { get; set; } = new();

        /// <summary>
        /// Custom Reply-To Email
        /// </summary>
        public string? ReplyTo { get; set; }

        /// <summary>
        /// Custom headers
        /// </summary>
        public Dictionary<string, string> CustomHeaders { get; set; } = new();
    }

}
