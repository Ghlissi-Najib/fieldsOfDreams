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
    public class QRScansController : ControllerBase
    {
        private readonly IQRScanService _qrScanService;

        public QRScansController(IQRScanService qrScanService)
        {
            _qrScanService = qrScanService;
        }

        [RequirePermission(RbacPolicies.ManageUsers)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _qrScanService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _qrScanService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var items = await _qrScanService.GetByUserIdAsync(userId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpGet("qrcode/{qrCodeId:guid}")]
        public async Task<IActionResult> GetByQRCode(Guid qrCodeId)
        {
            var items = await _qrScanService.GetByQRCodeIdAsync(qrCodeId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.PlayGames)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQRScanRequestDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var userEmail = User.Identity?.Name;
            try
            {
                var created = await _qrScanService.CreateAsync(dto, userId, userEmail);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _qrScanService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
