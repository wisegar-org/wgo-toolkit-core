using Wisegar.Toolkit.Models.Email;

namespace Wisegar.Toolkit.Services.Email
{
    public interface IEmailService
    {
        /// <summary>
        /// Send a complex email using the EmailMessage template
        /// </summary>
        /// <param name="emailMessage">Complete email template with all options</param>
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
