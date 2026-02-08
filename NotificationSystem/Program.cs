
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
var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "email.html");
//var templatePath = "C:\\Users\\Usman.Eijaz\\Documents\\Usman Backup\\Project\\NotificationSystem\\Templates\\email.html"; //Path.Combine("NotificationSystem\\Templates", "email.html");
var html = await File.ReadAllTextAsync(templatePath);

// Replace placeholders
html = html.Replace("{{name}}", "Usman");

// Send email
await emailService.SendEmailAsync(
    "test@yopmail.com",
    html,
    null // optional attachment path
);

Console.WriteLine("Process finished.");
