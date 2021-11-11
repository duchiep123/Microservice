﻿using AWS.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.AWS.SQS.Service
{
    public interface IAWSSQSService
    {
        Task<string> TestCreateQueue(string name);
        Task<bool> PostMessageAsync(string queueUrl, User user);
        Task<List<AllMessage>> GetAllMessagesAsync(string queueName);
        Task<bool> DeleteMessageAsync(DeleteMessage deleteMessage);
    }
}
