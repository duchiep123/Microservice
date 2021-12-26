using Serilog.Sinks.AwsCloudWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.CloudWatch
{
    public class AmazonCloudWatchLogStreamNameProvider : ILogStreamNameProvider
    {
        public string GetLogStreamName()
        {
            var now = DateTime.UtcNow;
            return $"{now.Year}-{now.Month}-{now.Day}";
        }
    }
}
