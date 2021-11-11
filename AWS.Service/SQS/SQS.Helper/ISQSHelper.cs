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
        Task<string> CreateSQSQueue(string name);
        Task<bool> SendMessageAsync(string queueURL, UserDetail userDetail);
        Task<List<Message>> ReceiveMessageAsync(string queueName);
        Task<bool> DeleteMessageAsync(string messageReceiptHandle);
        Task<List<string>> GetAllQueues();
    }
}
