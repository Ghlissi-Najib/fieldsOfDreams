using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRCodesController : ControllerBase
    {
        private readonly IQRCodeService _qrCodeService;

        public QRCodesController(IQRCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _qrCodeService.GetAllAsync();
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _qrCodeService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("by-code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var item = await _qrCodeService.GetByCodeAsync(code);
            return item is null ? NotFound() : Ok(item);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQRCodeRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _qrCodeService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQRCodeRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _qrCodeService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _qrCodeService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
