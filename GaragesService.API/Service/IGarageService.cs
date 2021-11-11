using GarageManagementModels;
using GaragesService.API.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.Service
{
    public interface IGarageService
    {
        Task<ResponseAddGarageModel> AddNewGarage();
        Garage GetGarageById(int id);
    }
}
