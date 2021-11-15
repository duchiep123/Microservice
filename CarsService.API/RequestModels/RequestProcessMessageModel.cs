using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.RequestModels
{
    public class RequestProcessMessageModel
    {
        public List<string> ReceiptHandles { get; set; }
        public string  QueueURL { get; set; }
    }
}
