using AWS.Service.Models;
using AWS.Service.SQS.SQS.Helper;
using CarsService.API.Repository;
using CarsService.API.RequestModels;
using CarsService.API.ResponseModel;
using GarageManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var check = await  CheckGarage(request.GarageId);
            if (check) {
                var result = await _carRepository.SaveChangesAsync();
                if (result > 0)
                {
                    return new ResponseAddCarModel() { Status = 0, Message = "Success", NewCar = car };
                }
                return new ResponseAddCarModel() { Status = 1, Message = "Failed", NewCar = null };

            }
            return new ResponseAddCarModel() { Status = 2, Message = "Garage is not found", NewCar = null };
        }

        public Car GetCarById(int id)
        {
            var result = _carRepository.GetCarById(id);
            return result;
        }

        private async Task<bool> CheckGarage(int garageId) {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var client = new HttpClient(clientHandler);
            var response = await client.GetAsync("https://garagesservice.api/api/garages/" + garageId);
            var tmp = await response.Content.ReadAsStringAsync();
            return String.IsNullOrEmpty(tmp) ? false : true;
        }
    }
}
