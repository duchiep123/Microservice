using CarsService.API.Controllers;
using CarsService.API.Repository;
using CarsService.API.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyUnitTest
{
    public class CarsControllerTests
    {
        private readonly Mock<ICarRepository> _mockRepo;
        private readonly Mock<ICarService> _mockService;
        private readonly CarsController _controller;

        public CarsControllerTests() {
            _mockRepo = new Mock<ICarRepository>();
            _mockService = new Mock<ICarService>();
            _controller = new CarsController(_mockService.Object);


        }

        [Fact]
        public void AddCar_ActionExecutes_ReturnsCar()
        {
            int carId = 8;
            _mockRepo.Setup(repo => repo.GetCarById(carId))
             .Returns(new GarageManagementModels.Car());
            
            var result = _controller.GetById(carId);

            
        }

    }
}
