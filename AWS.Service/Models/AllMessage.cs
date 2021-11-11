using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.Models
{
    public class AllMessage
    {
        public AllMessage()
        {
            UserDetail = new UserDetail();
        }
        public string MessageId { get; set; }
        public string ReceiptHandle { get; set; }

        public UserDetail UserDetail { get; set; }
    }
}
