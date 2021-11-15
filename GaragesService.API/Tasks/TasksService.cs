using AWS.Service.Models;
using AWS.Service.SQS.SQS.Helper;
using GaragesService.API.Repository;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GaragesService.API.Tasks
{
    public class TasksService
    {
        private readonly ILogger<QueueReaderService> _logger;
        private readonly ISQSHelper _sqsHelper;
        private readonly IGarageRepository _garageRepository;

        public TasksService(ILogger<QueueReaderService> logger, ISQSHelper sqsMessage, IGarageRepository garageRepository)
        {
            _logger = logger;
            _sqsHelper = sqsMessage;
            _garageRepository = garageRepository;
        }

        public async Task Cancelable(int iterationCount, IJobCancellationToken token)
        {
            try
            {
                for (var i = 1; i <= iterationCount; i++)
                {
                    await Task.Delay(1000);
                    Console.WriteLine("Performing step {0} of {1}...", i, iterationCount);

                    token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancellation requested, exiting...");
                throw;
            }
        }
        public async Task RunProcessMessages(string message, CancellationToken token)
        {
            try
            {
                Console.WriteLine("Run Job Processing Message");
                Console.WriteLine(message);
                await Task.Delay(100, token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancellation requested, exiting...");
                throw;
            }
        }
        public void StopTask(CancellationToken cancellationToken)
        {
            Console.WriteLine("stoped");
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async Task ReadQueue(string queueUrl) {
            var messages = await _sqsHelper.ReceiveMessageAsync(queueUrl);
            foreach (var item in messages)
            {
                try
                {
                    var content = JsonSerializer.Deserialize<CustomMessageBody>(item.Body);
                    if (content.Message.Contains("Get all"))
                    {
                        var result = _garageRepository.GetGarages();
                       
                        foreach (var item2 in result)
                        {
                            Console.WriteLine(item2.Name);
                        }
                        var deleteResult =  await _sqsHelper.DeleteMessageAsync(item.ReceiptHandle, queueUrl);
                        if (deleteResult) {
                            Console.WriteLine("Done");
                        }
                        Console.WriteLine("--------------------------------");
                    }
                    else
                    {
                        Console.WriteLine(content.Message);
                        // sau 3 lan thi vo dlq
                        
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        
        }


    }
}
