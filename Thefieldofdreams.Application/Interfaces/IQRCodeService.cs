using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IQRCodeService
    {
        Task<IEnumerable<QRCodeDto>> GetAllAsync();
        Task<QRCodeDto?> GetByIdAsync(Guid id);
        Task<QRCodeDto?> GetByCodeAsync(string code);
        Task<QRCodeDto> CreateAsync(CreateQRCodeRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateQRCodeRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
