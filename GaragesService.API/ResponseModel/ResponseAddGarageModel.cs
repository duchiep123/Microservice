using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.ResponseModel
{
    public class ResponseAddGarageModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Garage Garage{ get; set; }
    }
}
