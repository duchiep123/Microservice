using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AWS.Service.SQS.SQS.Helper
{
    public class SQSHelper : ISQSHelper
    {
        private readonly IAmazonSQS _sqs;
        public SQSHelper(
           IAmazonSQS sqs
            )
        {
            _sqs = sqs;
        }
        private static async Task<string> GetQueueArn(IAmazonSQS sqsClient, string qUrl)
        {
            GetQueueAttributesResponse responseGetAtt = await sqsClient.GetQueueAttributesAsync(
              qUrl, new List<string> { QueueAttributeName.QueueArn });
            return responseGetAtt.QueueARN;
        }

        public async Task<string> CreateSQSQueue(
            string name,
            string ReceiveMessageWaitTimeSeconds,
            string maxReceiveCount,
            string deadLetterQueueUrl = null
            )
        {
            var attrs = new Dictionary<string, string>();
            attrs.Add(QueueAttributeName.ReceiveMessageWaitTimeSeconds, ReceiveMessageWaitTimeSeconds);
            if (!string.IsNullOrEmpty(deadLetterQueueUrl))
            {
                var queueArnDeadLetter = await GetQueueArn(_sqs, deadLetterQueueUrl);
                var redrivePolicy = new { deadLetterTargetArn = queueArnDeadLetter, maxReceiveCount = maxReceiveCount };
                /*attrs.Add(QueueAttributeName.RedrivePolicy,
                  $"{{\"deadLetterTargetArn\":\"{await GetQueueArn(_sqs, deadLetterQueueUrl)}\"," +
                  $"\"maxReceiveCount\":\"{maxReceiveCount}\"}}");*/

                attrs.Add(QueueAttributeName.RedrivePolicy,JsonSerializer.Serialize(redrivePolicy));
                // Add other attributes for the message queue such as VisibilityTimeout
            }
            else {
                var resultCreateDeadLetterQueue = await _sqs.CreateQueueAsync(new CreateQueueRequest() // VisibilityTimeout mac dinh la 30s
                {
                    QueueName = name+"-dlq"
                });
                var dqlQueueArn = await GetQueueArn(_sqs, resultCreateDeadLetterQueue.QueueUrl);
                var redrivePolicy = new { deadLetterTargetArn = dqlQueueArn, maxReceiveCount = maxReceiveCount };
                attrs.Add(QueueAttributeName.RedrivePolicy, JsonSerializer.Serialize(redrivePolicy));

            }
            var resultCreateQueue = await _sqs.CreateQueueAsync(new CreateQueueRequest() // VisibilityTimeout mac dinh la 30s
            {
                QueueName = name,
                Attributes = attrs
            });
            
            // https://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_GetQueueAttributes.html
            return resultCreateQueue.HttpStatusCode == System.Net.HttpStatusCode.OK ? resultCreateQueue.QueueUrl : "Create queue failed.";
        }

        // SNS à một hệ thốg theo dõi công khai (publish-subscribe).
        // Những tin nhắn được đẩy đến người đăng ký khi và khi họ được nhà xuất bản gửi đến SNS.

        public async Task<bool> SendMessageAsync(string queueURL, UserDetail userDetail)
        {
            try
            {
                string message = JsonSerializer.Serialize(userDetail);
                var sendRequest = new SendMessageRequest(queueURL, message);
                // Post message or payload to queue  
                var sendResult = await _sqs.SendMessageAsync(sendRequest);
                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Message>> ReceiveMessageAsync(string queueUrl)
        {
            try
            {
                //Create New instance  
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = queueUrl,
                    MaxNumberOfMessages = 2,
                    WaitTimeSeconds = 5
                };
                //CheckIs there any new message available to process  
                var result = await _sqs.ReceiveMessageAsync(request);

                return result.Messages.Any() ? result.Messages : new List<Message>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteMessageAsync(string messageReceiptHandle, string queueUrl)
        {
            try
            {
                
                //Deletes the specified message from the specified queue  
                var deleteResult = await _sqs.DeleteMessageAsync(queueUrl, messageReceiptHandle);
                return deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetAllQueues()
        {
            var result = await _sqs.ListQueuesAsync(new ListQueuesRequest() { MaxResults = 10, QueueNamePrefix = "" });
            return result.QueueUrls;
        }
    }
}
