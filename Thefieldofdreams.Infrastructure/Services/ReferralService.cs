using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class ReferralService : IReferralService
    {
        private readonly ApplicationDbContext _context;

        public ReferralService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReferralDto>> GetAllAsync()
        {
            var items = await _context.Referrals.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<ReferralDto>> GetByReferrerIdAsync(Guid referrerUserId)
        {
            var items = await _context.Referrals.Where(r => r.ReferrerUserId == referrerUserId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<ReferralDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Referrals.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<ReferralDto> CreateAsync(CreateReferralRequestDto dto, string? createdBy)
        {
            var entity = new Referral
            {
                ReferrerUserId = dto.ReferrerUserId,
                ReferredUserId = dto.ReferredUserId,
                PointsAwarded = dto.PointsAwarded,
                IsCompleted = false,
                CreatedBy = createdBy
            };

            _context.Referrals.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Referrals.FindAsync(id);
            if (entity is null) return false;

            _context.Referrals.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ReferralDto MapToDto(Referral r) => new()
        {
            Id = r.Id,
            ReferrerUserId = r.ReferrerUserId,
            ReferredUserId = r.ReferredUserId,
            PointsAwarded = r.PointsAwarded,
            IsCompleted = r.IsCompleted,
            CreatedAt = r.CreatedAt
        };
    }
}
