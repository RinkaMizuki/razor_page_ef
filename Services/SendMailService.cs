using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace razor_page_ef
{
    public class SendMailService : IEmailSender
    {
        private IOptions<MailSettings> _options { get; set; }
        private ILogger<SendMailService> _logger { get; set; }
        public SendMailService(IOptions<MailSettings> option, ILogger<SendMailService> logger)
        {
            _options = option;
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.EmailAddress));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await smtp.ConnectAsync(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_options.Value.EmailAddress, _options.Value.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception e)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                Console.WriteLine($"emailsavefile ::: {emailsavefile}");
                await message.WriteToAsync(emailsavefile);

                _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                _logger.LogError(e.Message);
            }
            smtp.Disconnect(true);

            _logger.LogInformation("Send mail to: " + email);
        }
    }
}