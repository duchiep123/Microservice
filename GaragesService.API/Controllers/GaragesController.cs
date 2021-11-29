using GaragesService.API.RequestModels;
using GaragesService.API.Service;
using GaragesService.API.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaragesService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GaragesController : ControllerBase
    {

        IGarageService _garageService;
        IDistributedCache _redisCache;
        public GaragesController(IGarageService garageService, IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _garageService = garageService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _garageService.GetGarageById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestCreateGarageModel request)
        {
            if (ModelState.IsValid) {
                var result = await _garageService.AddNewGarage(request);
                if (result.Status == 0) {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(ModelState);
            
        }

        [HttpGet("readqueue/{queueName}")]
        public IActionResult Read(string queueName)
        {
            RecurringJob.AddOrUpdate<TasksService>(t => t.ReadQueue("http://localhost:4566/000000000000/"+queueName), "*/20 * * * * * ");

            return Ok();
        }

        // GET api/<GaragesController>/5
        [HttpGet("redis/{id}")]
        public string GetInRedis(int id)
        {
            _redisCache.SetString("test","nguyen duc hiep "+id);
            return "value";
        }

        [HttpGet("getdataredis")]
        public string GetData()
        {
            return _redisCache.GetString("test");
        }
    }
}
