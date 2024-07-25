﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Contract.IEmailContract
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
    }
}
