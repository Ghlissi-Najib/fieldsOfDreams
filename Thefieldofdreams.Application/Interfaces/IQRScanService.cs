using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IQRScanService
    {
        Task<IEnumerable<QRScanDto>> GetAllAsync();
        Task<IEnumerable<QRScanDto>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<QRScanDto>> GetByQRCodeIdAsync(Guid qrCodeId);
        Task<QRScanDto?> GetByIdAsync(Guid id);
        Task<QRScanDto> CreateAsync(CreateQRScanRequestDto dto, Guid userId, string? createdBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
