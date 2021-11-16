using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.RequestModels
{
    public class RequestCreateMusic
    {
        public string Artist { get; set; }
        public string SongTitle { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
    }
}
