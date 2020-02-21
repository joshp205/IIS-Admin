using IISAdministration.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace IISAdministration.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string server;
        private readonly int port;
        private readonly string fromEmail;
        private readonly string username;
        private readonly string password;
        private readonly string senderName;

        public EmailSender(IOptions<EmailConfiguration> emailConfiguration)
        {
            var config = emailConfiguration.Value;
            if (string.IsNullOrWhiteSpace(config.Email)
                || string.IsNullOrWhiteSpace(config.SmtpUsername)
                || string.IsNullOrWhiteSpace(config.SmtpPassword)
                || string.IsNullOrWhiteSpace(config.SenderName))
                throw new ArgumentNullException("The Values have not been set in the Secrets Manager");
            server = config.SmtpServer;
            port = config.SmtpPort;
            fromEmail = config.Email;
            senderName = config.SenderName;
            username = config.SmtpUsername;
            password = config.SmtpPassword;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(senderName, fromEmail));

            mimeMessage.To.Add(new MailboxAddress(email));

            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(server, port, false);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
