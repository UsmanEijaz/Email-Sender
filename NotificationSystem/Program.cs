
using EmailSender.Model;
using EmailSender.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<EmailSettings>(
            context.Configuration.GetSection("EmailSettings"));

        services.AddTransient<EmailService>();
        services.AddLogging();
    })
    .Build();

var emailService = host.Services.GetRequiredService<EmailService>();

// Load HTML template
var templatePath = "C:\\Users\\Usman.Eijaz\\Documents\\Usman Backup\\Project\\NotificationSystem\\Templates\\email.html"; //Path.Combine("NotificationSystem\\Templates", "email.html");
var html = await File.ReadAllTextAsync(templatePath);

// Replace placeholders
html = html.Replace("{{name}}", "Usman");

// Send email
await emailService.SendEmailAsync(
    "usman@almadinagroup.net",
    "Test Console Email",
    html,
    null // optional attachment path
);

Console.WriteLine("Process finished.");

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var email = new MimeMessage();

//        email.From.Add(new MailboxAddress("Sender Name", "your_email@gmail.com"));
//        email.To.Add(new MailboxAddress("Receiver", "receiver@gmail.com"));

//        email.Subject = "Test Email from Console App";
//        email.Body = new TextPart("plain")
//        {
//            Text = "Hello, this email is sent from .NET console app."
//        };

//        using var smtp = new SmtpClient();

//        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

//        // Use App Password, not normal Gmail password
//        await smtp.AuthenticateAsync("your_email@gmail.com", "your_app_password");

//        await smtp.SendAsync(email);
//        await smtp.DisconnectAsync(true);

//        Console.WriteLine("Email sent successfully!");
//    }
//}
