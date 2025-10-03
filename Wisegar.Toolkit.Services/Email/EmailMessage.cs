namespace Wisegar.Toolkit.Services.Email
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

    /// <summary>
    /// Email priority
    /// </summary>
    public enum EmailPriority
    {
        Low,
        Normal,
        High
    }

    /// <summary>
    /// Represents an attachment
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File content in bytes
        /// </summary>
        public byte[] Content { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// MIME type of the file
        /// </summary>
        public string ContentType { get; set; } = "application/octet-stream";

        /// <summary>
        /// Constructor to create an attachment from a file
        /// </summary>
        public EmailAttachment(string fileName, byte[] content, string contentType = "application/octet-stream")
        {
            FileName = fileName;
            Content = content;
            ContentType = contentType;
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public EmailAttachment() { }
    }
}