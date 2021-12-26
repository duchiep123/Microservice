﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Service.MailService
{
    public interface IMailService
    {
        Task<bool> SendMail(List<string> toEmails, string msgBody, string subject);
    }
}