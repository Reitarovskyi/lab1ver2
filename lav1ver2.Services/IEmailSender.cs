﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1ver2.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string name, string email, string subject, string message);
    }
}
