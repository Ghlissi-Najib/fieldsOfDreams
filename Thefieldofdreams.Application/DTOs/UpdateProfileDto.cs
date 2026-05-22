using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Application.DTOs
{
    public class UpdateProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? ProfilePhotoUrl { get; set; }
    }
}
