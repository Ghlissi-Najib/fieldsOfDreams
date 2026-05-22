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
    public class UserRewardsController : ControllerBase
    {
        private readonly IUserRewardService _userRewardService;

        public UserRewardsController(IUserRewardService userRewardService)
        {
            _userRewardService = userRewardService;
        }

        [RequirePermission(RbacPolicies.ManageUsers)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _userRewardService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _userRewardService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var items = await _userRewardService.GetByUserIdAsync(userId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRewardRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _userRewardService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRewardRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _userRewardService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _userRewardService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
