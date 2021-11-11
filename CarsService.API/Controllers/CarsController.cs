using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CarsService.API.Service;
using GarageManagementModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        ICarService _carService;
        IAmazonS3 _s3;
        public CarsController(ICarService carService, IAmazonS3 s3) {
            _carService = carService;
            _s3 = s3;
        }
        // GET: api/<CarsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _carService.AddNewCar();

            return Ok(result);
        }

        // GET api/<CarsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Car car = new Car();
            var result = _carService.GetCarById(id);
            return Ok(result);
        }

        // POST api/<CarsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            if (ModelState.IsValid) {
                 
            
            }
        }

        [HttpGet("create-bucker/{name}")]
        public async Task<IActionResult> CreateBucket(string name)
        {
            var createResponse = await _s3.PutBucketAsync(name);
            string source = Directory.GetCurrentDirectory(); // /src/CarsService.API
            await new TransferUtility(_s3).UploadAsync(source + "/test.txt", name, "some/path/image.jpg");
            var request = new GetObjectRequest()
            {
                BucketName = name,
                Key = "image.jpg"
            };
            if (createResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("buckets")]
        public async Task<IActionResult> GetAllBuckets()
        {
            List<object> returnValues = new List<object>();
            var result = await _s3.ListBucketsAsync();
            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var b in result.Buckets) {
                    ListObjectsRequest listRequest = new ListObjectsRequest
                    {
                        BucketName = b.BucketName
                        
                    };
                   var r = await _s3.ListObjectsAsync(listRequest);
                    returnValues.Add(new { BucketName = b.BucketName, objects = r });
                }
                return Ok(returnValues);
            }
            return BadRequest();
        }    

        // DELETE api/<CarsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
