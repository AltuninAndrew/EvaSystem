using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace EvaSystem.Services.AuxiliaryHandlers
{
    public class SendEmailManager
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("EvaSyStem STANKIN", "evasystemstankin@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("evasystemstankin@yandex.ru", "evasystemstankin2");
                //await client.ConnectAsync("smtp-mail.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }

}

