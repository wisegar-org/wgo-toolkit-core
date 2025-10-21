using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisegar.Toolkit.Models.Email
{
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
