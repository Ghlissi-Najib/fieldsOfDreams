using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            var items = await _context.Set<Transaction>().ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<TransactionDto>> GetByWalletIdAsync(Guid walletId)
        {
            var items = await _context.Set<Transaction>().Where(t => t.WalletId == walletId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<TransactionDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<Transaction>().FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<TransactionDto> CreateAsync(CreateTransactionRequestDto dto, string? createdBy)
        {
            var entity = new Transaction
            {
                WalletId = dto.WalletId,
                Amount = dto.Amount,
                Type = dto.Type,
                Description = dto.Description,
                ReferenceId = dto.ReferenceId,
                Status = TransactionStatus.Pending,
                CreatedBy = createdBy
            };

            _context.Set<Transaction>().Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Set<Transaction>().FindAsync(id);
            if (entity is null) return false;

            _context.Set<Transaction>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static TransactionDto MapToDto(Transaction t) => new()
        {
            Id = t.Id,
            WalletId = t.WalletId,
            Amount = t.Amount,
            Type = t.Type,
            Description = t.Description,
            ReferenceId = t.ReferenceId,
            Status = t.Status,
            CreatedAt = t.CreatedAt
        };
    }
}
