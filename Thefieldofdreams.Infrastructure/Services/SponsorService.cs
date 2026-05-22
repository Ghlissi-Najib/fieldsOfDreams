using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ApplicationDbContext _context;

        public SponsorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SponsorDto>> GetAllAsync()
        {
            var items = await _context.Sponsors.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<SponsorDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Sponsors.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<SponsorDto> CreateAsync(CreateSponsorRequestDto dto, string? createdBy)
        {
            var entity = new Sponsor
            {
                CompanyName = dto.CompanyName,
                LogoUrl = dto.LogoUrl,
                Website = dto.Website,
                ContactEmail = dto.ContactEmail,
                IsActive = dto.IsActive,
                Budget = dto.Budget,
                CreatedBy = createdBy
            };

            _context.Sponsors.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateSponsorRequestDto dto, string? updatedBy)
        {
            var entity = await _context.Sponsors.FindAsync(id);
            if (entity is null) return false;

            entity.CompanyName = dto.CompanyName;
            entity.LogoUrl = dto.LogoUrl;
            entity.Website = dto.Website;
            entity.ContactEmail = dto.ContactEmail;
            entity.IsActive = dto.IsActive;
            entity.Budget = dto.Budget;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Sponsors.FindAsync(id);
            if (entity is null) return false;

            _context.Sponsors.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static SponsorDto MapToDto(Sponsor s) => new()
        {
            Id = s.Id,
            CompanyName = s.CompanyName,
            LogoUrl = s.LogoUrl,
            Website = s.Website,
            ContactEmail = s.ContactEmail,
            IsActive = s.IsActive,
            Budget = s.Budget,
            TotalCampaigns = s.TotalCampaigns
        };
    }
}
