using ArchPilot.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace ArchPilot.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Email sent to {To} with subject {Subject}", to, subject);
        return Task.CompletedTask;
    }
}
