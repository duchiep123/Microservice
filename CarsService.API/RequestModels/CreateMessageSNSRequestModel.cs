using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.RequestModels
{
    public class CreateMessageSNSRequestModel
    {
        public string Topic { get; set; }
        public string Message { get; set; }
    }
}
