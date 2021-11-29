using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.Models
{
    public class CustomMessageBody
    {
        public string MessageId { get; set; }
        public string TopicArn { get; set; }
        public string Content { get; set; }
    }
}
