using GaragesService.API.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GaragesService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroudJobController : ControllerBase
    {
        // GET: api/<BackgroudJobController>
        [HttpGet("run")]
        public string Run()
        {
            RecurringJob.AddOrUpdate<TasksService>(t => t.RunProcessMessages("Hiep", CancellationToken.None), "*/2 * * * * * ");
            return "Ok";
        }

        [HttpGet("stop")]
        public string Stop()
        {
            BackgroundJob.Enqueue<TasksService>(t => t.StopTask(new CancellationToken(true)));
            return "Stoped";
        }

        // GET api/<BackgroudJobController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BackgroudJobController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BackgroudJobController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BackgroudJobController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
