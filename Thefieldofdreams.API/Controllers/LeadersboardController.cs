using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadersboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeadersboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        /// <summary>Returns leaderboard entries, optionally filtered by type.</summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTop([FromQuery] string? type = null, [FromQuery] int limit = 50)
        {
            var entries = await _leaderboardService.GetTopAsync(type, limit);
            return Ok(entries);
        }

        /// <summary>Returns one leaderboard entry by identifier.</summary>
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entry = await _leaderboardService.GetByIdAsync(id);
            return entry is null ? NotFound() : Ok(entry);
        }

        /// <summary>Creates a new leaderboard entry.</summary>
        [RequirePermission(RbacPolicies.ManageRewards)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaderboardEntryRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _leaderboardService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>Updates an existing leaderboard entry.</summary>
        [RequirePermission(RbacPolicies.ManageRewards)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLeaderboardEntryRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _leaderboardService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>Deletes a leaderboard entry.</summary>
        [RequirePermission(RbacPolicies.SelectWinners)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _leaderboardService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
