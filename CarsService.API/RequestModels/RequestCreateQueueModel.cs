using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.RequestModels
{
    public class RequestCreateQueueModel
    {
        public string Name { get; set; }
        public string ReceiveMessageWaitTimeSeconds { get; set; }
        public string MaxReceiveCount { get; set; }
        public string DeadLetterQueueUrl { get; set; }
    }
}
