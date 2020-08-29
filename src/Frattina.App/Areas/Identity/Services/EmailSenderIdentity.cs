using Frattina.CrossCutting.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Frattina.App.Areas.Identity.Services
{
    public class EmailSenderIdentity : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(AppSettings.Current.SendGridKey, subject, message, email);
        }

        private Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("site@frattina.com.br", "Frattina App"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
