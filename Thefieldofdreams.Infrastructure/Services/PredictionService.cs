using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly ApplicationDbContext _context;

        public PredictionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PredictionDto>> GetAllAsync()
        {
            var items = await _context.Predictions.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<PredictionDto>> GetByUserIdAsync(Guid userId)
        {
            var items = await _context.Predictions.Where(p => p.UserId == userId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<PredictionDto>> GetByMatchIdAsync(Guid matchId)
        {
            var items = await _context.Predictions.Where(p => p.MatchId == matchId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<PredictionDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Predictions.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<PredictionDto> CreateAsync(CreatePredictionRequestDto dto, Guid userId, string? createdBy)
        {
            var entity = new Prediction
            {
                UserId = userId,
                MatchId = dto.MatchId,
                HomeTeamScore = dto.HomeTeamScore,
                AwayTeamScore = dto.AwayTeamScore,
                WinnerPrediction = dto.WinnerPrediction,
                PredictedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            _context.Predictions.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdatePredictionRequestDto dto, string? updatedBy)
        {
            var entity = await _context.Predictions.FindAsync(id);
            if (entity is null) return false;

            entity.HomeTeamScore = dto.HomeTeamScore;
            entity.AwayTeamScore = dto.AwayTeamScore;
            entity.WinnerPrediction = dto.WinnerPrediction;
            entity.Status = dto.Status;
            entity.IsCorrect = dto.IsCorrect;
            entity.PointsEarned = dto.PointsEarned;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Predictions.FindAsync(id);
            if (entity is null) return false;

            _context.Predictions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static PredictionDto MapToDto(Prediction p) => new()
        {
            Id = p.Id,
            UserId = p.UserId,
            MatchId = p.MatchId,
            HomeTeamScore = p.HomeTeamScore,
            AwayTeamScore = p.AwayTeamScore,
            WinnerPrediction = p.WinnerPrediction,
            PointsEarned = p.PointsEarned,
            Status = p.Status,
            IsCorrect = p.IsCorrect,
            PredictedAt = p.PredictedAt
        };
    }
}
