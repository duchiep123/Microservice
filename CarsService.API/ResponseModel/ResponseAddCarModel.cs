using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.ResponseModel
{
    public class ResponseAddCarModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Car NewCar { get; set; }
    }
}
