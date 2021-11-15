using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.RequestModels
{
    public class SubscribeRequestModel
    {
        public string QueueName { get; set; }
        public string TopicName { get; set; }
    }
}
