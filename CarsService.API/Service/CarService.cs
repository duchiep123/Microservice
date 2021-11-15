using AWS.Service.Models;
using AWS.Service.SQS.SQS.Helper;
using CarsService.API.Repository;
using CarsService.API.RequestModels;
using CarsService.API.ResponseModel;
using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.Service
{
    public class CarService : ICarService
    {
        ICarRepository _carRepository;
        public CarService(ICarRepository carRepository) {
            _carRepository = carRepository;
        }
        public async Task<ResponseAddCarModel> AddNewCar(RequestCreateCarModel request)
        {
            Car car = new Car();
            car.Name = request.Name;
            car.Brand = request.Brand;
            car.Color = request.Color;
            _carRepository.AddCar(car);
            var result = await _carRepository.SaveChangesAsync();
            if (result > 0) {
                return new ResponseAddCarModel() { Status = 0, Message = "Success", NewCar = car };
            }
            return new ResponseAddCarModel() { Status = 1, Message = "Failed", NewCar = null};
        }

        public Car GetCarById(int id)
        {
            var result = _carRepository.GetCarById(id);
            return result;
        }
    }
}
