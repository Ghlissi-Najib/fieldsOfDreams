using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _campaignService.GetAllAsync();
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _campaignService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("sponsor/{sponsorId:guid}")]
        public async Task<IActionResult> GetBySponsor(Guid sponsorId)
        {
            var items = await _campaignService.GetBySponsorIdAsync(sponsorId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCampaignRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _campaignService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCampaignRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _campaignService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _campaignService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
