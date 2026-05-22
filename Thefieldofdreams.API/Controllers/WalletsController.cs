using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _walletService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var item = await _walletService.GetByUserIdAsync(userId);
            return item is null ? NotFound() : Ok(item);
        }

        [RequirePermission(RbacPolicies.ManageUsers)]
        [HttpPost("user/{userId:guid}")]
        public async Task<IActionResult> CreateForUser(Guid userId)
        {
            var userEmail = User.Identity?.Name;
            var created = await _walletService.CreateForUserAsync(userId, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _walletService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
