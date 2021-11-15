using CarsService.API.RequestModels;
using CarsService.API.ResponseModel;
using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.Service
{
    public interface ICarService
    {
        Task<ResponseAddCarModel> AddNewCar(RequestCreateCarModel request);
        Car GetCarById(int id);
    }
}
