using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        private readonly ApplicationDbContext _context;

        public QRCodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QRCodeDto>> GetAllAsync()
        {
            var items = await _context.QRCodes.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<QRCodeDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.QRCodes.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<QRCodeDto?> GetByCodeAsync(string code)
        {
            var entity = await _context.QRCodes.FirstOrDefaultAsync(q => q.Code == code);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<QRCodeDto> CreateAsync(CreateQRCodeRequestDto dto, string? createdBy)
        {
            var entity = new QRCode
            {
                Code = dto.Code,
                Title = dto.Title,
                Description = dto.Description,
                Type = dto.Type,
                SponsorId = dto.SponsorId,
                LocationId = dto.LocationId,
                PointsReward = dto.PointsReward,
                MaxScans = dto.MaxScans,
                ExpiryDate = dto.ExpiryDate,
                IsLimitedTime = dto.IsLimitedTime,
                CreatedBy = createdBy
            };

            _context.QRCodes.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateQRCodeRequestDto dto, string? updatedBy)
        {
            var entity = await _context.QRCodes.FindAsync(id);
            if (entity is null) return false;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Type = dto.Type;
            entity.SponsorId = dto.SponsorId;
            entity.LocationId = dto.LocationId;
            entity.PointsReward = dto.PointsReward;
            entity.MaxScans = dto.MaxScans;
            entity.ExpiryDate = dto.ExpiryDate;
            entity.IsLimitedTime = dto.IsLimitedTime;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.QRCodes.FindAsync(id);
            if (entity is null) return false;

            _context.QRCodes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static QRCodeDto MapToDto(QRCode q) => new()
        {
            Id = q.Id,
            Code = q.Code,
            Title = q.Title,
            Description = q.Description,
            Type = q.Type,
            SponsorId = q.SponsorId,
            LocationId = q.LocationId,
            PointsReward = q.PointsReward,
            MaxScans = q.MaxScans,
            CurrentScanCount = q.CurrentScanCount,
            ExpiryDate = q.ExpiryDate,
            IsLimitedTime = q.IsLimitedTime
        };
    }
}
