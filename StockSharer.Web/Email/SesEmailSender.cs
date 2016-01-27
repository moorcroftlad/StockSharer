using System.Collections.Generic;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace StockSharer.Web.Email
{
    public class SesEmailSender : ISendEmail
    {
        private const string FromEmail = "noreply@stocksharer.com";

        public void SendEmail(string to, string subject, string bodyText, string bodyHtml)
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest1))
            {
                var content = new Content(subject);
                var body = new Body{Html = new Content(bodyHtml), Text = new Content(bodyText)};
                var message = new Message(content, body);
                var destination = new Destination(new List<string> {to});
                var sendEmailRequest = new SendEmailRequest(FromEmail, destination, message);
                client.SendEmail(sendEmailRequest);
            }
        }
    }
}