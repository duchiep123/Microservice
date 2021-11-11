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
        ISQSHelper _sqsHelper;
        public CarService(ICarRepository carRepository, ISQSHelper sqsHelper) {
            _carRepository = carRepository;
            _sqsHelper = sqsHelper;
        }
        public async Task<ResponseAddCarModel> AddNewCar()
        {
            Car car = new Car();
            car.Name = "Alax";
            car.Brand = "BMW";
            car.Color = "Blue";
            _carRepository.AddCar(car);
            await _carRepository.SaveChangesAsync();
            var result = await _sqsHelper.CreateSQSQueue("demo_queue");
            Console.WriteLine("Queue: " + result);
            UserDetail userDetail = new UserDetail();
            userDetail.Id = new Random().Next(999999999);
            userDetail.FirstName = "Nguyen Duc";
            userDetail.LastName = "Hiep";
            userDetail.UserName = "mrhiep314";
            userDetail.EmailId = "mrhiep314@gmail.com";
            userDetail.CreatedOn = DateTime.UtcNow;
            userDetail.UpdatedOn = DateTime.UtcNow;
            var resultSendMes = await _sqsHelper.SendMessageAsync(result, userDetail);
            Console.WriteLine("Result send mes: "+resultSendMes);
            return new ResponseAddCarModel() { Status = 0, Message = "Success", NewCar = car };
        }

        public Car GetCarById(int id)
        {
            var result = _carRepository.GetCarById(id);
            return result;
        }
    }
}
