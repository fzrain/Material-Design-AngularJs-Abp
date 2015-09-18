using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace Fzrain.Service
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new MailAddress("fzrain@hotmail.com", "fzrain");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential("fzy55601", "fzy86087108");
            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);
            // Send the email.
            try
            {
                await transportWeb.DeliverAsync(myMessage);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.ToString());
            }

        }     
    }
}
