using AWS.Service.SQS.SQS.Helper;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GaragesService.API.Tasks
{
    public class QueueReaderService : BackgroundService
    {
        private readonly ILogger<QueueReaderService> _logger;
        private readonly ISQSHelper _sqsHelper;

        public QueueReaderService(ILogger<QueueReaderService> logger, ISQSHelper sqsMessage)
        {
            _logger = logger;
            _sqsHelper = sqsMessage;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("QueueReaderService is starting.");
            Console.WriteLine("QueueReaderService is starting.");
            try
            {
                while (!stoppingToken.IsCancellationRequested) // keep reading while the app is running.
                {
                    try
                    {
                        Console.WriteLine("Running..");
                        var messages = await _sqsHelper.ReceiveMessageAsync("garage_queue");
                        if (messages != null)
                        {
                            foreach (var message in messages)
                            {
                                BackgroundJob.Enqueue<TasksService>(
                                    x => x.RunProcessMessages(message.Body, stoppingToken));
                                await _sqsHelper.DeleteMessageAsync(message.ReceiptHandle, "queueUrl");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
