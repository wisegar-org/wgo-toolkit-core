namespace Wisegar.Toolkit.Services.Email
{
    /// <summary>
    /// Examples of using the generic email service
    /// </summary>
    public static class EmailServiceUsageExamples
    {
        /// <summary>
        /// Simple email example
        /// </summary>
        public static async Task SendSimpleEmailExample(IEmailService emailService)
        {
            await emailService.SendEmailAsync(
                to: "user@example.com",
                subject: "Welcome to the system",
                body: "Hello, welcome to our system.",
                isHtml: false
            );
        }

        /// <summary>
        /// Example of HTML email to multiple recipients
        /// </summary>
        public static async Task SendHtmlEmailExample(IEmailService emailService)
        {
            var destinatarios = new[] { "admin@company.com", "support@company.com" };
            var htmlBody = @"
                <html>
                <body>
                    <h2>Daily Report</h2>
                    <p>This is the daily report of the system.</p>
                    <ul>
                        <li>Active users: 150</li>
                        <li>Transactions: 45</li>
                        <li>Errors: 2</li>
                    </ul>
                </body>
                </html>";

            await emailService.SendEmailAsync(
                to: destinatarios,
                subject: "Daily System Report",
                body: htmlBody,
                isHtml: true
            );
        }

        /// <summary>
        /// Complex email example with attachments and advanced options
        /// </summary>
        public static async Task SendComplexEmailExample(IEmailService emailService)
        {
            // Create a sample attachment
            var reportContent = System.Text.Encoding.UTF8.GetBytes("Contents of the report...");
            var attachment = new EmailAttachment("report.txt", reportContent, "text/plain");

            var emailMessage = new EmailMessage
            {
                To = new List<string> { "manager@company.com", "CEO@company.com" },
                Cc = new List<string> { "admin@company.com" },
                Bcc = new List<string> { "audit@company.com" },
                Subject = "Monthly Report - CONFIDENTIAL",
                Body = @"
                    <html>
                    <body>
                        <h2 style='color: #2c5aa0;'>Monthly Report</h2>
                        <p>Dear,</p>
                        <p>Attached you will find the monthly report with the key indicators.</p>
                        <div style='background-color: #f0f8ff; padding: 10px; border-left: 4px solid #2c5aa0;'>
                            <strong>Executive Summary:</strong>
                            <ul>
                                <li>Sales: +15% vs previous month</li>
                                <li>New users: 1,250</li>
                                <li>Satisfaction: 4.8/5</li>
                            </ul>
                        </div>
                        <p>Kind regards,<br/>The analysis team</p>
                    </body>
                    </html>",
                IsHtml = true,
                Priority = EmailPriority.High,
                Attachments = new List<EmailAttachment> { attachment },
                ReplyTo = "noreply@company.com",
                CustomHeaders = new Dictionary<string, string>
                {
                    { "X-Category", "Monthly-Report" },
                    { "X-Department", "Analytics" }
                }
            };

            await emailService.SendEmailAsync(emailMessage);
        }

        /// <summary>
        /// Error notification example (specific use of the Monitor Service)
        /// </summary>
        public static async Task SendErrorNotificationExample(IEmailService emailService)
        {
            await emailService.SendErrorNotificationAsync(
                websiteUrl: "https://mycompany.com",
                statusCode: 500,
                status: "ERROR",
                errorMessage: "Internal Server Error - Database connection failed"
            );
        }

        /// <summary>
        /// Welcome email example with template
        /// </summary>
        public static async Task SendWelcomeEmailExample(IEmailService emailService, string userName, string userEmail)
        {
            var welcomeTemplate = $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                        <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                            <h1 style='margin: 0; font-size: 28px;'>Welcome!</h1>
                        </div>
                        <div style='background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px;'>
                            <h2 style='color: #667eea;'>Hello {userName},</h2>
                            <p>We're pleased to welcome you to our platform. Your account has been successfully created.</p>
                            
                            <div style='background: white; padding: 20px; border-radius: 5px; margin: 20px 0; border-left: 4px solid #667eea;'>
                                <h3 style='margin-top: 0; color: #667eea;'>Next steps:</h3>
                                <ul>
                                    <li>Complete your profile</li>
                                    <li>Explore our features</li>
                                    <li>Contact support if you need help</li>
                                </ul>
                            </div>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <a href='https://mycompany.com/dashboard' style='background: #667eea; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block;'>
                                    Access my account
                                </a>
                            </div>
                            
                            <p style='font-size: 14px; color: #666; text-align: center;'>
                               If you have any questions, please feel free to contact us at support@mycompany.com
                            </p>
                        </div>
                    </div>
                </body>
                </html>";

            await emailService.SendEmailAsync(
                to: userEmail,
                subject: "¡Welcome to our platform!",
                body: welcomeTemplate,
                isHtml: true
            );
        }
    }
}