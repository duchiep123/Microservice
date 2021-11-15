using Amazon.SQS.Model;
using AWS.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.SQS.SQS.Helper
{
    public interface ISQSHelper
    {
        Task<string> CreateSQSQueue(string name,
            string ReceiveMessageWaitTimeSeconds,
            string maxReceiveCount,
            string deadLetterQueueUrl = null);
        Task<bool> SendMessageAsync(string queueURL, UserDetail userDetail);
        Task<List<Message>> ReceiveMessageAsync(string queueUrl);
        Task<bool> DeleteMessageAsync(string messageReceiptHandle, string queueUrl);
        Task<List<string>> GetAllQueues();
    }
}
