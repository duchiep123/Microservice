﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API.RequestModels
{
    public class CreateMessageSNSRequestModel
    {
        public string TopicARN { get; set; }
        public string Message { get; set; }
    }
}
