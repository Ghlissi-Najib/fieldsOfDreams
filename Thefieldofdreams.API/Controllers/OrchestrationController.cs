using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Infrastructure.Identity;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrchestrationController : ControllerBase
    {
        private readonly ICampaignOrchestrator _orchestrator;
        private readonly UserManager<AppUser> _userManager;

        public OrchestrationController(ICampaignOrchestrator orchestrator, UserManager<AppUser> userManager)
        {
            _orchestrator = orchestrator;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns the per-user orchestration state: active campaigns, visible rewards,
        /// valid QR codes, and which flows are enabled for the caller's role.
        /// </summary>
        [HttpGet("state")]
        public async Task<IActionResult> GetState()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var appUser = await _userManager.FindByIdAsync(userIdClaim!);
            if (appUser is null)
                return Unauthorized();

            var role = appUser.Role.ToString().ToLowerInvariant();
            var state = await _orchestrator.GetOrchestrationStateAsync(userId, role, appUser.TotalPoints);

            return Ok(state);
        }
    }
}
