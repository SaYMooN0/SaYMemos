using MailKit.Net.Smtp;
using MimeKit;
using SaYMemos.Services.interfaces;

namespace SaYMemos.Services.implementations
{
    public class EmailService : IEmailService
    {
        private readonly string
            _smtpHost,
            _smtpUser,
            _smtpPassword;
        private readonly int _smtpPort;
        private readonly interfaces.ILogger _logger;

        public EmailService(string smtpHost, string smtpUser, string smtpPassword, int smtpPort, interfaces.ILogger logger)
        {
            _smtpHost = smtpHost;
            _smtpUser = smtpUser;
            _smtpPassword = smtpPassword;
            _smtpPort = smtpPort;
            _logger = logger;
        }

        public bool TrySendConfirmationCode(string email, string code) =>
            TrySendMessage(email, "Confirmation Code", $"<p>Your confirmation code is: <h2>{code}</h2></p>");


        public bool TrySendMessage(string email, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SayMemos", _smtpUser));
            message.To.Add(new MailboxAddress("To", email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };
            try
            {

                using (var client = ConfigureSmtpClient())
                {
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to send email to {email}", ex);
                return false;
            }
            _logger.Info("Email has been successfully sent to " + email);
            return true;
        }
        private SmtpClient ConfigureSmtpClient()
        {
            var client = new SmtpClient();
            client.Connect(_smtpHost, _smtpPort, true);
            client.Authenticate(_smtpUser, _smtpPassword);
            return client;
        }

    }
}


