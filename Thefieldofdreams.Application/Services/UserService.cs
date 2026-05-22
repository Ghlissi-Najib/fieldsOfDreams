using System;
using System.Collections.Generic;
using System.Text;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityService _identityService;

        public UserService(IIdentityService identityService) => _identityService = identityService;

        public async Task<IEnumerable<UserDto>> GetAllAsync()
            => await _identityService.GetAllUsersWithRolesAsync();
    }
}
