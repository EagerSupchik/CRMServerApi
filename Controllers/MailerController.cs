using CRMServerApi.Data;
using CRMServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace CRMServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public MailerController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/mailer/{email}/{subject}/{content}
        [HttpGet("{email}/{subject}/{content}")]
        public async Task<IActionResult> SendMail(string email, string subject, string content)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    _configuration["SmtpSettings:UserLabel"], 
                    _configuration["SmtpSettings:User"]));

                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = content
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        _configuration["SmtpSettings:Host"], 
                        Int32.Parse(_configuration["SmtpSettings:Port"]), 
                        SecureSocketOptions.StartTls
                    );

                    await client.AuthenticateAsync(
                        _configuration["SmtpSettings:User"], 
                        _configuration["SmtpSettings:Password"]
                    );

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return Content("Message was sent to the recipient");
            }
            catch (Exception ex)
            {
                return Content("Error in SendMail: " + ex.Message);
            }
        }

        // POST: api/mailer/send-mail-to-list
        [HttpPost("send-mail-to-list")]
        public async Task<IActionResult> SendToList([FromBody] EmailRequest request)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        _configuration["SmtpSettings:Host"], 
                        Int32.Parse(_configuration["SmtpSettings:Port"]), 
                        SecureSocketOptions.StartTls
                    );

                    await client.AuthenticateAsync(
                        _configuration["SmtpSettings:User"], 
                        _configuration["SmtpSettings:Password"]
                    );

                    foreach (var email in request.Emails)
                    {
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress(
                            _configuration["SmtpSettings:UserLabel"], 
                            _configuration["SmtpSettings:User"]));

                        message.To.Add(new MailboxAddress(email, email));
                        message.Subject = request.Subject;
                        message.Body = new TextPart("plain")
                        {
                            Text = request.Content
                        };

                        await client.SendAsync(message);
                    }

                    await client.DisconnectAsync(true);
                }

                return Ok("Messages were sent to the recipients");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error in send-mail-to-list: " + ex.Message);
            }
        }
    }
}
