using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.RequestModels
{
    public class RequestCreateGarageModel
    {
        public string Address { get; set; }
        public string Name { get; set; }

    }
}
