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

        [HttpPost("createqueue")]
        public async Task<IActionResult> CreateQueue(RequestCreateQueueModel request)
        {
            var result = await _AWSSQSService.TestCreateQueue(request.Name, request.ReceiveMessageWaitTimeSeconds, request.MaxReceiveCount, request.DeadLetterQueueUrl);
            return Ok(result);
        }

        [HttpGet("getattriubutes/{name}")]
        public async Task<IActionResult> GetAttriute(string name )
        {
            string queueUrl = "http://localhost:4566/000000000000/" + name;
            var result = await _sqs.GetAttributesAsync(queueUrl);
            return Ok(result);
        }



        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe(SubscribeRequestModel subscribeRequestModel)
        {
            var result = await _sns.SubscribeSQStoSNS(subscribeRequestModel.TopicName, subscribeRequestModel.QueueName, _sqs);
            return Ok(result);
        }

        
        [Route("getAllMessages/{queueName}")]
        [HttpGet]
        public async Task<IActionResult> GetAllMessagesAsync(string queueName)
        {
            var result = await _AWSSQSService.GetAllMessagesAsync(queueName);
            return Ok(result);
        }
    }
}
