using GarageManagementModels;
using GarageManagementModels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.ResponseModel
{
    public class ReturnGetGarageModel
    {
        public int GarageId { get; set; }
        public string GarageName { get; set; }
        public string Address { get; set; }
        public List<CarReadDto> Cars{ get; set; }
    }
}
