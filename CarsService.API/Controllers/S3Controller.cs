using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        IAmazonS3 _s3;
        public S3Controller(IAmazonS3 s3) {
            _s3 = s3;
        }

        [HttpGet("create-bucker/{name}")]
        public async Task<IActionResult> CreateBucket(string name)
        {
            var createResponse = await _s3.PutBucketAsync(name);
            if (createResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, string bucketName)
        {
            
            var stream = file.OpenReadStream();
           // string source = Directory.GetCurrentDirectory(); // /src/CarsService.API
            string fileName = string.Empty;
            PutObjectResponse response = null;
            //string fileExtension = Path.GetExtension(source + "/test.txt");
            var listTmp = file.FileName.Split(".");
            string fileExt = listTmp[listTmp.Length - 1];
            fileName = $"{DateTime.Now.Ticks}.{fileExt}";
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                BucketName = bucketName,
                Key = fileName
            };
            response = await _s3.PutObjectAsync(request);

            // await new TransferUtility(_s3).UploadAsync(source + "/matching.png", name, fileName);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
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
                foreach (var b in result.Buckets)
                {
                    ListObjectsRequest listRequest = new ListObjectsRequest
                    {
                        BucketName = b.BucketName

                    };
                    var r = await _s3.ListObjectsAsync(listRequest);
                    returnValues.Add(new { BucketName = b.BucketName, info = r });
                }
                return Ok(returnValues);
            }
            return BadRequest();
        }

        [HttpGet("download/{bucketName}/{fileName}")]
        public async Task<IActionResult> Download(string bucketName, string fileName)
        {
            var objectResponse = await _s3.GetObjectAsync(bucketName, fileName);
            return File(objectResponse.ResponseStream, objectResponse.Headers.ContentType, fileName);
        }
    }
}
