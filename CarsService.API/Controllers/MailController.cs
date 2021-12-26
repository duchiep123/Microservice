using AWS.Service.MailService;
using AWS.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        IMailService _mailService;

        public MailController(IMailService mailService) {
            _mailService = mailService;
        }
        //(SES does not support FIFO type topics.)
        [HttpPost]
        public async Task<IActionResult> Post(SendMailModel request)
        {
            if (ModelState.IsValid)
            {
                var result = await _mailService.SendMail(new List<string> { request.ToEmail }, request.Message, request.Subject);
                if (result) {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }
    }
}
