using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
    }
}
