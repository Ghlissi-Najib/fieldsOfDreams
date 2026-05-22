using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;

namespace Thefieldofdreams.Infrastructure.Services
{
    /// <summary>
    /// Leaderboard service implementation containing leaderboard CRUD business operations.
    /// </summary>
    public class LeaderboardService : ILeaderboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaderboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LeaderboardEntryDto>> GetTopAsync(string? leaderboardType, int limit)
        {
            var normalizedLimit = Math.Clamp(limit, 1, 100);
            var entries = await _unitOfWork.LeaderboardEntries.GetTopAsync(leaderboardType, normalizedLimit);
            return entries.Select(MapToDto);
        }

        public async Task<LeaderboardEntryDto?> GetByIdAsync(Guid id)
        {
            var entry = await _unitOfWork.LeaderboardEntries.GetByIdAsync(id);
            return entry is null ? null : MapToDto(entry);
        }

        public async Task<LeaderboardEntryDto> CreateAsync(CreateLeaderboardEntryRequestDto dto, string? createdBy)
        {
            var entry = new LeaderboardEntry
            {
                UserId = dto.UserId,
                Username = dto.Username,
                TotalPoints = dto.TotalPoints,
                WeeklyPoints = dto.WeeklyPoints,
                MonthlyPoints = dto.MonthlyPoints,
                Rank = dto.Rank,
                LeaderboardType = string.IsNullOrWhiteSpace(dto.LeaderboardType) ? "Global" : dto.LeaderboardType,
                LastUpdated = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await _unitOfWork.LeaderboardEntries.CreateAsync(entry);
            _unitOfWork.Complete();
            return MapToDto(entry);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateLeaderboardEntryRequestDto dto, string? updatedBy)
        {
            var entry = await _unitOfWork.LeaderboardEntries.GetByIdAsync(id);
            if (entry is null)
            {
                return false;
            }

            entry.Username = dto.Username;
            entry.TotalPoints = dto.TotalPoints;
            entry.WeeklyPoints = dto.WeeklyPoints;
            entry.MonthlyPoints = dto.MonthlyPoints;
            entry.Rank = dto.Rank;
            entry.LeaderboardType = string.IsNullOrWhiteSpace(dto.LeaderboardType) ? "Global" : dto.LeaderboardType;
            entry.LastUpdated = DateTime.UtcNow;
            entry.UpdatedAt = DateTime.UtcNow;
            entry.UpdatedBy = updatedBy;

            await _unitOfWork.LeaderboardEntries.UpdateAsync(entry);
            _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entry = await _unitOfWork.LeaderboardEntries.GetByIdAsync(id);
            if (entry is null)
            {
                return false;
            }

            await _unitOfWork.LeaderboardEntries.DeleteAsync(id);
            _unitOfWork.Complete();
            return true;
        }

        private static LeaderboardEntryDto MapToDto(LeaderboardEntry entry)
        {
            return new LeaderboardEntryDto
            {
                Id = entry.Id,
                UserId = entry.UserId,
                Username = entry.Username,
                TotalPoints = entry.TotalPoints,
                WeeklyPoints = entry.WeeklyPoints,
                MonthlyPoints = entry.MonthlyPoints,
                Rank = entry.Rank,
                LeaderboardType = entry.LeaderboardType,
                LastUpdated = entry.LastUpdated
            };
        }
    }
}
