using ECommerce.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ECommerce.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
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
            //await smtpClient.SendMailAsync(message);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            var mail = new StringBuilder();
            mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br>");
            mail.AppendLine("<strong><a target=\"_blank\" href=\"");
            mail.Append(_configuration["ECommerceClientUrl"]);
            mail.Append("/update-password/");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong></br></br><span style=\"font-size:12px;\">NOT: Bu talep sizin tarafınızdan oluşturulmadı ise lütfen dikkate almayınız</span>");
            await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
        }

        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName)
        {
            var mail = new StringBuilder();
            mail.AppendLine("Sayın ").Append(userName).Append(" Merhaba<br>").Append(orderDate).Append(" tarihinde vermiş olduğunuz ")
                .Append(orderCode).Append(" kodlu siparişiniz tamamlanmış ve kargo firmasına teslim edilmiştir.");

            await SendMailAsync(to, $"{orderCode} numaralı Siparişiniz Hakkında", mail.ToString());
        }
    }
}
