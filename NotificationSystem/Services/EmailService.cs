using EmailSender.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

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
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(
                    _settings.DisplayName,
                    _settings.From));

                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = htmlBody
                };

                if (!string.IsNullOrEmpty(attachmentPath)
                    && File.Exists(attachmentPath))
                {
                    builder.Attachments.Add(attachmentPath);
                }

                email.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_settings.Username, _settings.Password);

                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }

                //using var smtp = new SmtpClient();


                //await smtp.ConnectAsync(
                //    _settings.Host,
                //    _settings.Port,
                //   SecureSocketOptions.SslOnConnect);

                ////await smtp.ConnectAsync(
                ////    _settings.Host,
                ////    _settings.Port,
                ////   SecureSocketOptions.StartTls);

                //await smtp.AuthenticateAsync(
                //    _settings.Username,
                //    _settings.Password);

                //await smtp.SendAsync(email);
                //await smtp.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email sending failed");
            }
        }
    }
}
