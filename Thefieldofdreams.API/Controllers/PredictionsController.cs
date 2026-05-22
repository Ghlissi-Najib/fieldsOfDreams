using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PredictionsController : ControllerBase
    {
        private readonly IPredictionService _predictionService;

        public PredictionsController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _predictionService.GetAllAsync();
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _predictionService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var items = await _predictionService.GetByUserIdAsync(userId);
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("match/{matchId:guid}")]
        public async Task<IActionResult> GetByMatchId(Guid matchId)
        {
            var items = await _predictionService.GetByMatchIdAsync(matchId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.PlayGames)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePredictionRequestDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var userEmail = User.Identity?.Name;
            var created = await _predictionService.CreateAsync(dto, userId, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.ManageRewards)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePredictionRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _predictionService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _predictionService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
