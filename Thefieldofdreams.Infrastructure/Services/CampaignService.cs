using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ApplicationDbContext _context;

        public CampaignService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CampaignDto>> GetAllAsync()
        {
            var items = await _context.Campaigns.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<CampaignDto>> GetBySponsorIdAsync(Guid sponsorId)
        {
            var items = await _context.Campaigns.Where(c => c.SponsorId == sponsorId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<CampaignDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Campaigns.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<CampaignDto> CreateAsync(CreateCampaignRequestDto dto, string? createdBy)
        {
            var entity = new Campaign
            {
                Name = dto.Name,
                Description = dto.Description,
                SponsorId = dto.SponsorId,
                Type = dto.Type,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Budget = dto.Budget,
                Status = dto.Status,
                Priority = dto.Priority,
                CreatedBy = createdBy
            };

            _context.Campaigns.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateCampaignRequestDto dto, string? updatedBy)
        {
            var entity = await _context.Campaigns.FindAsync(id);
            if (entity is null) return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Type = dto.Type;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.Budget = dto.Budget;
            entity.Status = dto.Status;
            entity.Priority = dto.Priority;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Campaigns.FindAsync(id);
            if (entity is null) return false;

            _context.Campaigns.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static CampaignDto MapToDto(Campaign c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            SponsorId = c.SponsorId,
            Type = c.Type,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Budget = c.Budget,
            TotalImpressions = c.TotalImpressions,
            TotalClicks = c.TotalClicks,
            TotalConversions = c.TotalConversions,
            Status = c.Status,
            Priority = c.Priority
        };
    }
}
