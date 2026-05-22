using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext _context;

        public WalletService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WalletDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Wallets.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<WalletDto?> GetByUserIdAsync(Guid userId)
        {
            var entity = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<WalletDto> CreateForUserAsync(Guid userId, string? createdBy)
        {
            var entity = new Wallet
            {
                UserId = userId,
                Balance = 0,
                Points = 0,
                CreatedBy = createdBy
            };

            _context.Wallets.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Wallets.FindAsync(id);
            if (entity is null) return false;

            _context.Wallets.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static WalletDto MapToDto(Wallet w) => new()
        {
            Id = w.Id,
            UserId = w.UserId,
            Balance = w.Balance,
            Points = w.Points,
            StripeCustomerId = w.StripeCustomerId
        };
    }
}
