using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>Returns the profile of the currently authenticated user.</summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrent()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var profile = await _accountService.GetCurrentUserProfileAsync(userId);
            return profile is null ? NotFound() : Ok(profile);
        }

        /// <summary>Updates the profile of the currently authenticated user.</summary>
        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrent([FromBody] UpdateAccountRequestDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var updated = await _accountService.UpdateCurrentUserProfileAsync(userId, dto);
            return updated ? NoContent() : NotFound();
        }
    }
}
