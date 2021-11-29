using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.SNS.SNS.Helper
{
    public class SNSHelper : ISNSHelper
    {
        private readonly IAmazonSimpleNotificationService _simpleNotificationService;

        public SNSHelper(IAmazonSimpleNotificationService amazonSimpleNotificationService)
        {
            _simpleNotificationService = amazonSimpleNotificationService;
        }

        public async Task<string> CreateTopicSNS(string topicName)
        {
            if (!String.IsNullOrEmpty(topicName))
            {
                var resultTopic = await _simpleNotificationService.CreateTopicAsync(topicName);
                return resultTopic.HttpStatusCode == System.Net.HttpStatusCode.OK ? resultTopic.TopicArn : "";
            }
            return "";
        }

        public async Task<string> SubscribeSQStoSNS(string topicName, string queueName, IAmazonSQS amazonSQS)
        {
            var topic = await _simpleNotificationService.FindTopicAsync(topicName);
            if (topic != null)
            {
                var sqs = await amazonSQS.GetQueueUrlAsync(queueName);
                var result = await _simpleNotificationService.SubscribeQueueAsync(topic.TopicArn, amazonSQS, sqs.QueueUrl);
                return result;
            }
            return "";
        }

        public async Task<string> SendMessageToSNS(string message, string topicArn)
        {
            var request = new PublishRequest    
            {
                Message = message,
                TopicArn = topicArn

            };
            // When you subscribe an Amazon SQS queue to an SNS topic, Amazon SNS uses HTTPS to forward messages to Amazon SQS
            var response = await _simpleNotificationService.PublishAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK ? response.MessageId : "";
        }

        public async Task<List<string>> GetAllTopics()
        {
            var result = await _simpleNotificationService.ListTopicsAsync();
            List<string> returnValue = new List<string>();
            foreach (var t in result.Topics) {
                returnValue.Add(t.TopicArn);
            }
            return returnValue;
        }
    }
}
