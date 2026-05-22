using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRCampaignsController : ControllerBase
    {
        private readonly IQRCampaignService _qrCampaignService;

        public QRCampaignsController(IQRCampaignService qrCampaignService)
        {
            _qrCampaignService = qrCampaignService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _qrCampaignService.GetAllAsync();
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _qrCampaignService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("campaign/{campaignId:guid}")]
        public async Task<IActionResult> GetByCampaign(Guid campaignId)
        {
            var items = await _qrCampaignService.GetByCampaignIdAsync(campaignId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQRCampaignRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _qrCampaignService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQRCampaignRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _qrCampaignService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _qrCampaignService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
