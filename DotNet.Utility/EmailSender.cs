﻿using System;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DotNet.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Logic to send email
            return Task.CompletedTask;
        }
    }
}

