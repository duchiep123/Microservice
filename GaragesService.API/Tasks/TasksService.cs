using AWS.Service.SQS.SQS.Helper;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GaragesService.API.Tasks
{
    public class TasksService
    {
        private readonly ILogger<QueueReaderService> _logger;
        private readonly ISQSHelper _sqsHelper;

        public TasksService(ILogger<QueueReaderService> logger, ISQSHelper sqsMessage)
        {
            _logger = logger;
            _sqsHelper = sqsMessage;
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
    }
}
