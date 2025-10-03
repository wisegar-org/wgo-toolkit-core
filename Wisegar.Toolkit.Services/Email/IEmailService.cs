namespace Wisegar.Toolkit.Services.Email
{
    public interface IEmailService
    {
        /// <summary>
        /// Send a generic email with the specified parameters
        /// </summary>
        /// <param name="to">Email recipients</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body (can be HTML)</param>
        /// <param name="isHtml">Indicates whether the body is HTML</param>
        Task SendEmailAsync(string[] to, string subject, string body, bool isHtml = false);

        /// <summary>
        /// Send a generic email to a single recipient
        /// </summary>
        /// <param name="to">Email recipient</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body (can be HTML)</param>
        /// <param name="isHtml">Indicates whether the body is HTML</param>
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);

        /// <summary>
        /// Send a complex email using the EmailMessage template
        /// </summary>
        /// <param name="emailMessage">Complete email template with all options</param>
        Task SendEmailAsync(EmailMessage emailMessage);

        /// <summary>
        /// Sends a specific error notification for website monitoring
        /// </summary>
        /// <param name="websiteUrl">Website URL</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="status">Status of the site (ONLINE/OFFLINE/ERROR/TIMEOUT)</param>
        /// <param name="errorMessage">Optional error message</param>
        Task SendErrorNotificationAsync(string websiteUrl, int statusCode, string status, string? errorMessage = null);
    }
}
