using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.MailService
{
    public class MailService : IMailService
    {
        IAmazonSimpleEmailService _sescClient;
        public MailService(IAmazonSimpleEmailService sescClient ) {
            //AmazonSimpleEmailServiceClient sesclient = new AmazonSimpleEmailServiceClient();
            //ListVerifiedEmailAddressesRequest addrsrqst = new ListVerifiedEmailAddressesRequest();
            //ListVerifiedEmailAddressesResponse adrsrsp = sesclient.ListVerifiedEmailAddressesAsync(addrsrqst).Result;
            _sescClient = sescClient;
        }

        public async Task<bool> SendMail(List<string> toEmails, string msgBody, string subject) {
            string body = "Test email Sent from amazon Ses :" + msgBody;
            Body bdy = new Body();
            bdy.Html = new Content(body);
            Content title = new Content(subject);
            Message msg = new Message(title, bdy);
            Destination destination = new Destination() {
                ToAddresses = toEmails
            };
            try{
                SendEmailRequest sndrqst = new SendEmailRequest(source: "hiepnguyen4999@gmail.com", destination, msg);
                SendEmailResponse sndrsp = await _sescClient.SendEmailAsync(sndrqst);
                return sndrsp.HttpStatusCode == System.Net.HttpStatusCode.OK ? true : false;
	    }
	    catch(Exception ex){
		Console.WriteLine(ex.Message);
		return false;
}
            
        }
    }
}
