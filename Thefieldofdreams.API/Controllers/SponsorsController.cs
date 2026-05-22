using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorsController : ControllerBase
    {
        private readonly ISponsorService _sponsorService;

        public SponsorsController(ISponsorService sponsorService)
        {
            _sponsorService = sponsorService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _sponsorService.GetAllAsync();
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _sponsorService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [RequirePermission(RbacPolicies.ManagePartners)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSponsorRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _sponsorService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.ManagePartners)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSponsorRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _sponsorService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _sponsorService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
