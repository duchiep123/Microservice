using Amazon.SQS;
using AWS.Service.Models;
using AWS.Service.SNS;
using AWS.Service.SQS.SQS.Helper;
using CarsService.API.AWS.SQS.Service;
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
    public class AWSSQSController : ControllerBase
    {
        private readonly IAWSSQSService _AWSSQSService;
        private readonly ISNSHelper _sns;
        private readonly ISQSHelper _sqsHelper;
        private readonly IAmazonSQS _sqs;

        public AWSSQSController(IAWSSQSService AWSSQSService, ISNSHelper sns, IAmazonSQS sqs, ISQSHelper sqsHelper)
        {
            _AWSSQSService = AWSSQSService;
            _sns = sns;
            _sqs = sqs;
            _sqsHelper = sqsHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQueues()
        {
            var result = await _sqsHelper.GetAllQueues();
            return Ok(result);
        }

        [HttpGet("createqueue/{name}")]
        public async Task<IActionResult> CreateQueue(string name)
        {
            var result = await _AWSSQSService.TestCreateQueue(name);
            return Ok(result);
        }

        

        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe(SubscribeRequestModel subscribeRequestModel)
        {
            var result = await _sns.SubscribeSQStoSNS(subscribeRequestModel.Topic, subscribeRequestModel.QueueURL, _sqs);
            return Ok(result);
        }

        [Route("postMessage/{queueName}")]
        [HttpPost]
        public async Task<IActionResult> PostMessageAsync(User user, string queueName)
        {
            var result = await _AWSSQSService.PostMessageAsync(queueName, user);
            return Ok(new { isSucess = result });
        }
        [Route("getAllMessages/{queueName}")]
        [HttpGet]
        public async Task<IActionResult> GetAllMessagesAsync(string queueName)
        {
            var result = await _AWSSQSService.GetAllMessagesAsync(queueName);
            return Ok(result);
        }
        [Route("deleteMessage")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMessageAsync(DeleteMessage deleteMessage)
        {
            var result = await _AWSSQSService.DeleteMessageAsync(deleteMessage);
            return Ok(new { isSucess = result });
        }
    }
}
