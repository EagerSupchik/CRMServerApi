using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRMServerApi.Data;
using CRMServerApi.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace CRMServerApi.Services
{
    public class BirthdayEmailService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BirthdayEmailService> _logger;

        public BirthdayEmailService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<BirthdayEmailService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);
                    if (now > nextRunTime)
                    {
                        nextRunTime = nextRunTime.AddDays(1);
                    }
                    var delay = nextRunTime - now;

                    await Task.Delay(delay, stoppingToken);

                    await SendBirthdayMails(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in BirthdayEmailService: {ex}");
                }
            }
        }

        private async Task SendBirthdayMails(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var today = DateTime.Today;
                var birthdayUsers = await dbContext.Clients
                    .Where(u => u.BirthDate.HasValue && u.BirthDate.Value.Month == today.Month && u.BirthDate.Value.Day == today.Day)
                    .ToListAsync(stoppingToken);

                if (birthdayUsers == null || !birthdayUsers.Any())
                {
                    return;
                }

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        _configuration["SmtpSettings:Host"],
                        int.Parse(_configuration["SmtpSettings:Port"]),
                        SecureSocketOptions.StartTls,
                        stoppingToken
                    );
                    await client.AuthenticateAsync(
                        _configuration["SmtpSettings:User"],
                        _configuration["SmtpSettings:Password"],
                        stoppingToken
                    );

                    foreach (var user in birthdayUsers)
                    {
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress(_configuration["SmtpSettings:UserLabel"], _configuration["SmtpSettings:User"]));
                        message.To.Add(new MailboxAddress(user.Name, user.Email));
                        message.Subject = "Happy Birthday!";
                        message.Body = new TextPart("plain")
                        {
                            Text = $"{user.Name}, Happy Birthday!"
                        };

                        await client.SendAsync(message, stoppingToken);
                        _logger.LogInformation($"Email was sent to {user.Email}");
                    }

                    await client.DisconnectAsync(true, stoppingToken);
                }
            }
        }
    }
}