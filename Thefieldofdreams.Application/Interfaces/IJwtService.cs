using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(string userId, string email, string fullName, IList<string> roles);
        string GenerateRefreshToken();
        string? GetUserIdFromExpiredToken(string token);
    }
}
