using Frattina.CrossCutting.Configuration;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Frattina.CrossCutting.Email
{
    public class EmailSenderApp
    {
        public async Task<Response> send(Values values)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(values.fromEmail, values.fromName),
                Subject = values.subject,
                PlainTextContent = values.plainTextContent,
                HtmlContent = values.htmlContent
            };

            msg.AddTo(new EmailAddress(values.toEmail, values.toName));

            msg.SetClickTracking(false, false);

            var client = new SendGridClient(AppSettings.Current.SendGridKey);

            return await client.SendEmailAsync(msg);
        }
    }

    public class Values
    {
        public string fromEmail { get; set; }

        public string fromName { get; set; }

        public string toEmail { get; set; }

        public string toName { get; set; }

        public string subject { get; set; }

        public string plainTextContent { get; set; }

        public string htmlContent { get; set; }
    }
}
