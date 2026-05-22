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
    public class UserRouteCompletionsController : ControllerBase
    {
        private readonly IUserRouteCompletionService _service;

        public UserRouteCompletionsController(IUserRouteCompletionService service)
        {
            _service = service;
        }

        [RequirePermission(RbacPolicies.ViewFullAnalytics)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var items = await _service.GetByUserIdAsync(userId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.PlayGames)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRouteCompletionRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _service.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.ManageRewards)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRouteCompletionRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _service.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
