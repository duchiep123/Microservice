using AWS.Service.SNS;
using CarsService.API.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AWSSNSController : ControllerBase
    {
        private ISNSHelper _snsHelper;

        public AWSSNSController(ISNSHelper snsHelper) {
            _snsHelper = snsHelper;
        }
        // GET: api/<AWSSNSController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _snsHelper.GetAllTopics();
            return Ok(result);
        }

        // GET api/<AWSSNSController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("createSNS/{name}")]
        public async Task<IActionResult> CreateSNS(string name)
        {
            var result = await _snsHelper.CreateTopicSNS(name);
            return Ok(result);
        }
        // POST api/<AWSSNSController>
        [HttpPost]
        public async Task<IActionResult> CreateSNSMessage([FromBody] CreateMessageSNSRequestModel request)
        {
            var result = await _snsHelper.SendMessageToSNS(request.Message, request.Topic);
            return Ok(result);
        }

        // PUT api/<AWSSNSController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AWSSNSController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
