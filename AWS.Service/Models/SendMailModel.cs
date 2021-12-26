using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.Models
{
    public class SendMailModel
    {
        public string ToEmail{ get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }

    }
}
