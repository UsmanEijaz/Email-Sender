using EmailSender.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EmailSender.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings,
                            ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string htmlBody,
            string? attachmentPath = null)
        {
            try
            {
                var email = new MailMessage();
                email.From = new MailAddress(_settings.From);
                email.Subject = subject;
                email.To.Add(new MailAddress(toEmail));

                if (!string.IsNullOrEmpty(attachmentPath)
                    && File.Exists(attachmentPath))
                {
                    email.Attachments.Add(new Attachment(attachmentPath));
                }

                email.Body = htmlBody;
                email.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Port = _settings.Port;
                    smtp.Host = _settings.Host;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(_settings.From, _settings.Password);
                    //smtp.Send(email);
                    await smtp.SendMailAsync(email);
                }
                _logger.LogInformation("Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email sending failed");
            }
        }
    }
}
