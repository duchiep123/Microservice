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

        public async Task<string> CreateSQSQueue(string name)
        {
            var resultCreateQueue = await _sqs.CreateQueueAsync(new CreateQueueRequest() // VisibilityTimeout mac dinh la 30s
            {
                QueueName = name,
                Attributes = new Dictionary<string, string>
                     {
                        {"ReceiveMessageWaitTimeSeconds", "10"},
                     }
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
        public async Task<List<Message>> ReceiveMessageAsync(string queueName)
        {
            try
            {
                //Create New instance  
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = "http://localhost:4566/000000000000/"+queueName,
                    MaxNumberOfMessages = 10,
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
        public async Task<bool> DeleteMessageAsync(string messageReceiptHandle)
        {
            try
            {
                //Deletes the specified message from the specified queue  
                var deleteResult = await _sqs.DeleteMessageAsync("", messageReceiptHandle);
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
