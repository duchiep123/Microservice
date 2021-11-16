using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CarsService.API.RequestModels;
using CarsService.API.Service;
using GarageManagementModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
        private readonly IAmazonDynamoDB _amazonDynamoDB;
        public CarsController(ICarService carService, IAmazonS3 s3, IAmazonDynamoDB amazonDynamoDB) {
            _carService = carService;
            _s3 = s3;
            _amazonDynamoDB = amazonDynamoDB;
        }

        [HttpGet("{carId}")]
        public async Task<IActionResult> Get(int carId)
        {
            Table people = Table.LoadTable(_amazonDynamoDB, "Car");
            var person = JsonSerializer.Deserialize<Car>((await people.GetItemAsync(carId)).ToJson());
            return Ok(person);
        }

        [HttpPost("createMusicDynamoDB")]
        public async Task<IActionResult> CreateDynamoDB()
        {
            var request = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>() {
                    new AttributeDefinition
                    {
                        AttributeName = "Artist",
                        AttributeType = "S"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "SongTitle",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Artist",
                        KeyType = "HASH" //Partition key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "SongTitle",
                        KeyType = "RANGE" //Sort key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 6
                },
                TableName = "Music"
            };
            var response = await _amazonDynamoDB.CreateTableAsync(request);
            Console.WriteLine(response.HttpStatusCode);
            TableDescription description = response.TableDescription;
            Console.WriteLine("Name: {0}", description.TableName);
            Console.WriteLine("# of items: {0}", description.ItemCount);
            Console.WriteLine("Provision Throughput (reads/sec): {0}",
                      description.ProvisionedThroughput.ReadCapacityUnits);
            Console.WriteLine("Provision Throughput (writes/sec): {0}",
                      description.ProvisionedThroughput.WriteCapacityUnits);
            
            return Ok(response.TableDescription.TableStatus);
        }

        [HttpPost("createMusic")]
        public async Task<IActionResult> CreateMusic(RequestCreateMusic requestCreateMusic)
        {
            var request = new PutItemRequest
            {
                TableName = "Music",
                Item = new Dictionary<string, AttributeValue>()
                {
                    { "Artist", new AttributeValue { S = requestCreateMusic.Artist }},
                    { "SongTitle", new AttributeValue { S = requestCreateMusic.SongTitle }},
                    { "Album", new AttributeValue { S = requestCreateMusic.Album }},
                    { "Year", new AttributeValue { N = requestCreateMusic.Year.ToString() }},

                }
            };
            var result = await _amazonDynamoDB.PutItemAsync(request);
            return Ok(result);
        }

        [HttpGet("getMusic/{artist}/{songTitle}")]
        public async Task<IActionResult> GetMusic(string artist, string songTitle)
        {
            Primitive hash = new Primitive(artist, false);
            Primitive range = new Primitive(songTitle, false);

            Table musicTable = Table.LoadTable(_amazonDynamoDB, "Music");
            /*var request = new GetItemRequest
            {
                TableName = "Person",
                Key = new Dictionary<string, AttributeValue>()
            {
                { 
                   "Id", new AttributeValue { N =id } 
                },
                {
                   "Name", new AttributeValue { S =name}
                }
            },
                ProjectionExpression = "Id, Name",
                ConsistentRead = true
            };*/
            GetItemOperationConfig config = new GetItemOperationConfig
            {
                AttributesToGet = new List<string> { "Id", "Name" },
                ConsistentRead = true
            };
            //var result = await _amazonDynamoDB.GetItemAsync(hash,);
            var result = await musicTable.GetItemAsync(hash, range);
            var returnResult = result.ToJson();
            return Ok(returnResult);
        }

        [HttpGet("getMusic")]
        public async Task<IActionResult> GetMusicByQuery(string artist, string songTitle)
        {

            var request = new QueryRequest
            {
                TableName = "Music",
                KeyConditionExpression = "Artist = :v_Artist",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_Artist", new AttributeValue { S =  artist }}}
            };
            var result = await _amazonDynamoDB.QueryAsync(request);

            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK) {
                return Ok(result.Items);
            }
            return BadRequest();
        }

        // GET api/<CarsController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _carService.GetCarById(id);
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

        [HttpGet("uploadFile/{bucketName}")]
        public async Task<IActionResult> UploadFile(string bucketName)
        {
            string source = Directory.GetCurrentDirectory(); // /src/CarsService.API
            string fileName = string.Empty;
            PutObjectResponse response = null;
            using (FileStream fsSource = new FileStream(source + "/test.txt", FileMode.Open, FileAccess.Read))
            {
                string fileExtension = Path.GetExtension(source + "/test.txt");

                fileName = $"{DateTime.Now.Ticks}{fileExtension}";
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = fsSource,
                    BucketName = bucketName,
                    Key = fileName
                };
                response = await _s3.PutObjectAsync(request);
            }

            // await new TransferUtility(_s3).UploadAsync(source + "/matching.png", name, fileName);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("download/{bucketName}/{fileName}")]
        public async Task<IActionResult> Download(string bucketName, string fileName) {
            TransferUtility transferUtility = new TransferUtility(_s3);
            var objectResponse = await _s3.GetObjectAsync(bucketName, fileName);
            return File(objectResponse.ResponseStream, objectResponse.Headers.ContentType, fileName);
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
    }
}
