using CarsService.API.Controllers;
using CarsService.API.Models;
using CarsService.API.Repository;
using CarsService.API.Service;
using GarageManagementModels;
using GarageManagementModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private Mock<ICarRepository> _mockRepo = new Mock<ICarRepository>();
        private Mock<ICarService> _mockService = new Mock<ICarService>();
        private Mock<ILogger<CarsController>> _mockLogger = new Mock<ILogger<CarsController>>();
        private CarsController _controller;

        [Fact]
        public void GetCarById_ActionExecutes_ReturnsCar()
        {
            // Arrange
            var cars = GetSampleCars();
            int carId = 1;

            _mockService.Setup(services => services.GetCarById(carId)).Returns(cars[0]);
            _controller = new CarsController(_mockService.Object, _mockLogger.Object);
            
            // Act

            var actionResult = _controller.GetById(carId);
            // Assert
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal((((OkObjectResult)actionResult).Value as ReturnGetCarModel).CarId, carId);
        }

        [Fact]
        public void GetCarById_ActionExecutes_ReturnsNotFoundCar()
        {
            // Arrange
            var cars = GetSampleCars();
            int carId = 1;

            _mockService.Setup(services => services.GetCarById(carId)).Returns(cars[0]);
            _controller = new CarsController(_mockService.Object, _mockLogger.Object);

            // Act

            var actionResult = _controller.GetById(2);
            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }



        private List<ReturnGetCarModel> GetSampleCars()
        {
            List<ReturnGetCarModel> output = new List<ReturnGetCarModel>
        {
            new ReturnGetCarModel
            {
                CarBrand = "BMW",
                CarId = 1,
                CarColor = "Yellow",
                CarName = "BMW x3",
                GarageName = "Duc Hiep Garage",
                GarageId = 1
            },
            new ReturnGetCarModel
            {
                CarBrand = "Honda",
                CarId = 2,
                CarColor = "Red",
                CarName = "Tetst Honda",
                GarageName = "Duc Hiep Garage",
                GarageId = 1
            },
            new ReturnGetCarModel
            {
                CarBrand = "Honda",
                CarId = 3,
                CarColor = "Blue",
                CarName = "Honda future",
                GarageName = "Duc Hiep Garage",
                GarageId = 1
            }
        };
            return output;
        }

    }
}
