using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.Models
{
    public class ReturnGetCarModel
    {
        public int CarId { get; set; }
        public int GarageId { get; set; }
        public string CarName { get; set; }
        public string CarColor { get; set; }
        public string CarBrand { get; set; }
        public string GarageName { get; set; }

    }
}
