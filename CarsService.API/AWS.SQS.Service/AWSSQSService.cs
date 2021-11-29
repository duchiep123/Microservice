using Amazon.SQS.Model;
using AWS.Service.Models;
using AWS.Service.SQS.SQS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarsService.API.AWS.SQS.Service
{
    public class AWSSQSService : IAWSSQSService
    {
        private ISQSHelper _AWSSQSHelper;
        public AWSSQSService(ISQSHelper AWSSQSHelper)
        {
            _AWSSQSHelper = AWSSQSHelper;
        }


        public async Task<string> TestCreateQueue(string name,
            string ReceiveMessageWaitTimeSeconds,
            string maxReceiveCount,
            string deadLetterQueueUrl = null) {
            var result = await _AWSSQSHelper.CreateSQSQueue(name,ReceiveMessageWaitTimeSeconds, maxReceiveCount,deadLetterQueueUrl);
            return result;
        }
        public async Task<bool> PostMessageAsync(string queueUrl, User user)
        {
            try
            {
                UserDetail userDetail = new UserDetail();
                userDetail.Id = new Random().Next(999999999);
                userDetail.FirstName = user.FirstName;
                userDetail.LastName = user.LastName;
                userDetail.UserName = user.UserName;
                userDetail.EmailId = user.EmailId;
                userDetail.CreatedOn = DateTime.UtcNow;
                userDetail.UpdatedOn = DateTime.UtcNow;
                return await _AWSSQSHelper.SendMessageAsync(queueUrl, userDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<AllMessage>> GetAllMessagesAsync(string queueName)
        {
            List<AllMessage> allMessages = new List<AllMessage>();
            try
            {
                string url = "http://localhost:4566/000000000000/" + queueName;
                List<Message> messages = await _AWSSQSHelper.ReceiveMessageAsync(url);
                for (int i = 0; i < messages.Count; i++)
                {
                    
                    var result =   JsonSerializer.Deserialize<CustomMessageBody>(messages[i].Body);
                    Console.WriteLine(result.Content);
                }
                allMessages = messages.Select(c => new AllMessage { MessageId = c.MessageId, ReceiptHandle = c.ReceiptHandle, UserDetail = JsonSerializer.Deserialize<UserDetail>(c.Body) }).ToList();
                return allMessages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMessageAsync(DeleteMessage deleteMessage)
        {
            try
            {
                return await _AWSSQSHelper.DeleteMessageAsync(deleteMessage.ReceiptHandle, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
