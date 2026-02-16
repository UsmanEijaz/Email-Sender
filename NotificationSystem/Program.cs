using EmailSender.Model;
using EmailSender.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
var settings = host.Services.GetRequiredService<IOptions<EmailSettings>>().Value;

// Load HTML template
var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "email.html");
var html = await File.ReadAllTextAsync(templatePath);

// Replace placeholders
html = html.Replace("{{name}}", "Usman");

// Send email
await emailService.SendEmailAsync(
    settings.To,
    "Text Subject",
    html,
    null // optional attachment path
);