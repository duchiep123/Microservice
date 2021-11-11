using GaragesService.API.Service;
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
        // GET: api/<GaragesController>
        [HttpGet("callapi/{port}")]
        public async Task<IActionResult> Get(string port)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://carsservice.api:" + port+"/api/cars/1");
            if (response.IsSuccessStatusCode)
            {
               var result = response.Content;
                return Ok(result);
            }
            return BadRequest("Fail");

        }

        [HttpGet]
        public async Task<IActionResult> Get2()
        {
            var result = await _garageService.AddNewGarage();
            return Ok(result);
        }

        // GET api/<GaragesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            Console.WriteLine("Vo ne");
            _redisCache.SetString("test","nguyen duc hiep "+id);
            return "value";
        }

        [HttpGet("getdataredis")]
        public string GetData()
        {
            return _redisCache.GetString("test");
        }

        // POST api/<GaragesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GaragesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GaragesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
