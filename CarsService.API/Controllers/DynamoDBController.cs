using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using CarsService.API.RequestModels;
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
    public class DynamoDBController : ControllerBase
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;

        public DynamoDBController(IAmazonDynamoDB amazonDynamoDB) {
            _amazonDynamoDB = amazonDynamoDB;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable()
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

            if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(result.Items);
            }
            return BadRequest();
        }
    }
}
