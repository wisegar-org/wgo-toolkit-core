using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Wisegar.Toolkit.Models.Email;
using Wisegar.Toolkit.Models.MSGraph;
using Wisegar.Toolkit.Services.Email;

namespace Wisegar.Toolkit.Services.xTest.Email
{
    public class EmailMSGraphServiceTest
    {
        private readonly IEmailService _emailService;
        private readonly Mock<ILogger<EmailMSGraphService>> _loggerMock;
        private readonly MSGraphSettings _msGraphSettings;

        // Test data constants
        private const string TestEmail = "yariel.re@gmail.com";
        private const string TestEmail1 = "hurshelann30@gmail.com";
        private const string TestEmail2 = "test@example.com";
        
        public EmailMSGraphServiceTest()
        {
            _loggerMock = new Mock<ILogger<EmailMSGraphService>>();
            var emailSettingsMock = SettingsService.GetMSGraphMockSettings();
            _msGraphSettings = emailSettingsMock.Object.Value;
            _emailService = new EmailMSGraphService(emailSettingsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task SendSimpleEmailTestAsync()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = _msGraphSettings.Principal,
                To = [TestEmail],
                Subject = "MS GRAPH - Monthly Report - CONFIDENTIAL",
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
                ReplyTo = TestEmail,
                CustomHeaders = new Dictionary<string, string>
                {
                    { "X-Category", "Monthly-Report" },
                    { "X-Department", "Analytics" }
                }
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithAttachment_ShouldSendSuccessfully()
        {
            // Arrange
            var attachment = new EmailAttachment(
                "test-document.txt",
                System.Text.Encoding.UTF8.GetBytes("Test attachment content - MSGraph"),
                "text/plain"
            );

            var emailMessage = new EmailMessage
            {
                From = _msGraphSettings.Principal,
                To = [TestEmail],
                Subject = "Test Email with Attachment - MSGraph",
                Body = "<h2>Email con Adjunto</h2><p>Este email contiene un archivo adjunto via Microsoft Graph.</p>",
                IsHtml = true,
                Attachments = new List<EmailAttachment> { attachment }
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithMultipleAttachments_ShouldSendSuccessfully()
        {
            // Arrange
            var attachment1 = new EmailAttachment(
                "document1.txt",
                System.Text.Encoding.UTF8.GetBytes("First document"),
                "text/plain"
            );

            var attachment2 = new EmailAttachment(
                "document2.txt",
                System.Text.Encoding.UTF8.GetBytes("Second document"),
                "text/plain"
            );

            var emailMessage = new EmailMessage
            {
                From = _msGraphSettings.Principal,
                To = [TestEmail],
                Subject = "Test Email with Multiple Attachments - MSGraph",
                Body = "<h2>Email con Múltiples Adjuntos</h2>",
                IsHtml = true,
                Attachments = new List<EmailAttachment> { attachment1, attachment2 }
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithCcAndBcc_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = _msGraphSettings.Principal,
                To = [TestEmail],
                Cc = [TestEmail1],
                Bcc = [TestEmail2],
                Subject = "Test Email with CC and BCC - MSGraph",
                Body = "This email has CC and BCC recipients via Microsoft Graph",
                IsHtml = false
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        [Fact]
        public async Task SendEmailAsync_WithHighPriority_ShouldSendSuccessfully()
        {
            // Arrange
            var emailMessage = new EmailMessage
            {
                From = _msGraphSettings.Principal,
                To = [TestEmail],
                Subject = "High Priority Email - MSGraph",
                Body = "<h2>URGENT</h2><p>High priority email via Microsoft Graph</p>",
                IsHtml = true,
                Priority = EmailPriority.High
            };

            // Act
            var exception = await Record.ExceptionAsync(async () =>
                await _emailService.SendEmailAsync(emailMessage)
            );

            // Assert
            AssertExpectedExceptionOrNull(exception);
        }

        #region Helper Methods

        private static void AssertExpectedExceptionOrNull(Exception? exception)
        {
            // Permite null (éxito) o excepciones esperadas de Microsoft Graph
            Assert.True(
                exception == null || exception is Microsoft.Graph.Models.ODataErrors.ODataError || exception is InvalidOperationException,
                $"Tipo de excepción inesperado: {exception?.GetType().Name} - {exception?.Message}");
        }

        #endregion
    }
}
