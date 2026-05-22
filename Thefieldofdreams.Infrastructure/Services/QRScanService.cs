using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class QRScanService : IQRScanService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICampaignOrchestrator _orchestrator;

        public QRScanService(ApplicationDbContext context, ICampaignOrchestrator orchestrator)
        {
            _context = context;
            _orchestrator = orchestrator;
        }

        public async Task<IEnumerable<QRScanDto>> GetAllAsync()
        {
            var items = await _context.QRScans.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<QRScanDto>> GetByUserIdAsync(Guid userId)
        {
            var items = await _context.QRScans.Where(s => s.UserId == userId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<QRScanDto>> GetByQRCodeIdAsync(Guid qrCodeId)
        {
            var items = await _context.QRScans.Where(s => s.QRCodeId == qrCodeId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<QRScanDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.QRScans.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<QRScanDto> CreateAsync(CreateQRScanRequestDto dto, Guid userId, string? createdBy)
        {
            var isValid = await _orchestrator.IsQRCodeValidAsync(dto.QRCodeId, userId);
            if (!isValid)
                throw new InvalidOperationException("QR code is not valid or not available for scanning.");

            var qrCode = await _context.QRCodes.FindAsync(dto.QRCodeId);
            var pointsAwarded = qrCode?.PointsReward ?? 0;

            var entity = new QRScan
            {
                UserId = userId,
                QRCodeId = dto.QRCodeId,
                PointsAwarded = pointsAwarded,
                LocationData = dto.LocationData,
                DeviceInfo = dto.DeviceInfo,
                ScannedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            if (qrCode is not null)
                qrCode.CurrentScanCount++;

            _context.QRScans.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.QRScans.FindAsync(id);
            if (entity is null) return false;

            _context.QRScans.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static QRScanDto MapToDto(QRScan s) => new()
        {
            Id = s.Id,
            UserId = s.UserId,
            QRCodeId = s.QRCodeId,
            PointsAwarded = s.PointsAwarded,
            LocationData = s.LocationData,
            DeviceInfo = s.DeviceInfo,
            ScannedAt = s.ScannedAt
        };
    }
}
