using Serilog.Events;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.CloudWatch
{
    public class AmazonCloudWatchTextFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            logEvent.RenderMessage(output);
        }
    }
}
