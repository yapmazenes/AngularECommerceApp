using ECommerce.Application.Abstractions.Services;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMessageAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage message = new MailMessage();
            message.IsBodyHtml = isBodyHtml;
            message.Subject = subject;
            message.Body = body;
            for (int i = 0; i < tos.Length; i++) message.To.Add(tos[i]);
            message.From = new MailAddress(_configuration["MailSettings:Username"], "E-Commerce", Encoding.UTF8);

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(_configuration["MailSettings:Username"], _configuration["MailSettings:Password"]);
            smtpClient.Port = int.Parse(_configuration["MailSettings:Port"]);
            smtpClient.EnableSsl = true;
            smtpClient.Host = _configuration["MailSettings:Host"];
            await smtpClient.SendMailAsync(message);
        }
    }
}
