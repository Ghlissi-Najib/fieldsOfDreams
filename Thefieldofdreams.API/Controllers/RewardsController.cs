using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RewardsController : ControllerBase
    {
        private readonly IRewardService _rewardService;

        public RewardsController(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _rewardService.GetAllAsync();
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _rewardService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [RequirePermission(RbacPolicies.ManageRewards)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRewardRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _rewardService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.ManageRewards)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRewardRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _rewardService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _rewardService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
