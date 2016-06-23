using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Fzrain.Service
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            //var myMessage = new SendGridMessage();
            //myMessage.AddTo(message.Destination);
            //myMessage.From = new MailAddress("fzrain@hotmail.com", "fzrain");
            //myMessage.Subject = message.Subject;
            //myMessage.Text = message.Body;
            //myMessage.Html = message.Body;

            //var credentials = new NetworkCredential("fzy55601", "fzy86087108");
            //// Create a Web transport for sending email.
            //var transportWeb = new Web(credentials);
            //// Send the email.
            //await transportWeb.DeliverAsync(myMessage);

            String apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY", EnvironmentVariableTarget.User);
            dynamic sg = new SendGridAPIClient(apiKey);

            Email from = new Email("test@example.com");
            String subject = "Hello World from the SendGrid CSharp Library";
            Email to = new Email("test@example.com");
            Content content = new Content("text/plain", "Textual content");
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}
