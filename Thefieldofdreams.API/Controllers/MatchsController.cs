using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchsController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchsController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>Returns all matches.</summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var matches = await _matchService.GetAllAsync();
            return Ok(matches);
        }

        /// <summary>Returns a single match by identifier.</summary>
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var match = await _matchService.GetByIdAsync(id);
            return match is null ? NotFound() : Ok(match);
        }

        /// <summary>Creates a new match.</summary>
        [RequirePermission(RbacPolicies.PostLiveSlots)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMatchRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _matchService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>Updates an existing match.</summary>
        [RequirePermission(RbacPolicies.PostLiveSlots)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMatchRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _matchService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>Deletes an existing match.</summary>
        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _matchService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
