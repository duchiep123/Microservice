using Amazon.SQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.SNS
{
    public interface ISNSHelper
    {
        Task<string> CreateTopicSNS(string topicName);
        Task<string> SubscribeSQStoSNS(string snsTopic, string queueURL, IAmazonSQS amazonSQS);
        Task<string> SendMessageToSNS(string message, string topicArn);
        Task<List<string>> GetAllTopics();
    }
}
