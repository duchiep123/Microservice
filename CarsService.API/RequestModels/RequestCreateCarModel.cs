using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.RequestModels
{
    public class RequestCreateCarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public int GarageId { get; set; }
    }
}
