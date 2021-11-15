using GarageManagementModels;
using GaragesService.API.RequestModels;
using GaragesService.API.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GaragesService.API.Service
{
    public interface IGarageService
    {
        Task<ResponseAddGarageModel> AddNewGarage(RequestCreateGarageModel request);
        Garage GetGarageById(int id);
    }
}
