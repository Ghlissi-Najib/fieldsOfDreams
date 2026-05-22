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
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [RequirePermission(RbacPolicies.ViewFullAnalytics)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _transactionService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _transactionService.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("wallet/{walletId:guid}")]
        public async Task<IActionResult> GetByWallet(Guid walletId)
        {
            var items = await _transactionService.GetByWalletIdAsync(walletId);
            return Ok(items);
        }

        [RequirePermission(RbacPolicies.ManageRewards)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequestDto dto)
        {
            var userEmail = User.Identity?.Name;
            var created = await _transactionService.CreateAsync(dto, userEmail);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [RequireRole("Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _transactionService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
