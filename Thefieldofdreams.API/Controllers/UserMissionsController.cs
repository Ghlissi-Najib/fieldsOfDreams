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
    public class UserMissionsController : ControllerBase
    {
        private readonly IUserMissionService _userMissionService;

        public UserMissionsController(IUserMissionService userMissionService)
        {
            _userMissionService = userMissionService;
        }

        [RequirePermission(RbacPolicies.ManageUsers)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _userMissionService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _userMissionService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var items = await _userMissionService.GetByUserIdAsync(userId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.PlayGames)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserMissionRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _userMissionService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.PlayGames)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProgress(Guid id, [FromBody] UpdateUserMissionProgressDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _userMissionService.UpdateProgressAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _userMissionService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
