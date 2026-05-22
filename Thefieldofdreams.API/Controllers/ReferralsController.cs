using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReferralsController : ControllerBase
    {
        private readonly IReferralService _referralService;

        public ReferralsController(IReferralService referralService)
        {
            _referralService = referralService;
        }

        [RequirePermission(RbacPolicies.ManageUsers)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _referralService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _referralService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("referrer/{referrerUserId:guid}")]
        public async Task<IActionResult> GetByReferrer(Guid referrerUserId)
        {
            var items = await _referralService.GetByReferrerIdAsync(referrerUserId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.PlayGames)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReferralRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _referralService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _referralService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
