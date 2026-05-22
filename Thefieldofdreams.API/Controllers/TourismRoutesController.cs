using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourismRoutesController : ControllerBase
    {
        private readonly ITourismRouteService _tourismRouteService;

        public TourismRoutesController(ITourismRouteService tourismRouteService)
        {
            _tourismRouteService = tourismRouteService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _tourismRouteService.GetAllAsync();
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _tourismRouteService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [RequirePermission(RbacPolicies.PostLiveSlots)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTourismRouteRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _tourismRouteService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequirePermission(RbacPolicies.PostLiveSlots)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTourismRouteRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var updated = await _tourismRouteService.UpdateAsync(id, dto, userEmail);
            return updated ? NoContent() : NotFound();
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _tourismRouteService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
