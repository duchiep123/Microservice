using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.RequestModels
{
    public class SubscribeRequestModel
    {
        public string QueueURL { get; set; }
        public string Topic { get; set; }
    }
}
