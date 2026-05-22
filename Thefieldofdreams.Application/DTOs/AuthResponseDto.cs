using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Application.DTOs
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePhotoUrl { get; set; }
        public string Role { get; set; } = string.Empty;
        public Guid? MerchantId { get; set; }
        public Guid? PartnerId { get; set; }

        public string? CompanyName { get; set; }
        public Guid? ClientRecordId { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
