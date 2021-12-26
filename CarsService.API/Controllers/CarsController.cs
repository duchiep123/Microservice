using CarsService.API.RequestModels;
using CarsService.API.Service;
using GarageManagementModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        ICarService _carService;
        private readonly ILogger<CarsController> p_logger;
        public CarsController(ICarService carService, ILogger<CarsController> logger) {
            p_logger = logger;
            _carService = carService;
            p_logger.LogInformation("[CarsController] constructor");
        }
        

        // GET api/<CarsController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            p_logger.LogInformation("[CarsController] Start get car by id");
            var result = _carService.GetCarById(id);
            p_logger.LogInformation("[CarsController] Get car completed");
            if (result == null) {
                return NotFound();
            }
            return Ok(result);
        }

        // POST api/<CarsController>
        [HttpPost]
        public async Task<IActionResult> Post(RequestCreateCarModel request)
        {
            if (ModelState.IsValid) {
                var result = await _carService.AddNewCar(request);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}
