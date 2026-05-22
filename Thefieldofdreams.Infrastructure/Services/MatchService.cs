using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;

namespace Thefieldofdreams.Infrastructure.Services
{
    /// <summary>
    /// Match service implementation containing match CRUD business operations.
    /// </summary>
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MatchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MatchDto>> GetAllAsync()
        {
            var matches = await _unitOfWork.Matchs.GetAllMatchesAsync();
            return matches.Select(MapToDto);
        }

        public async Task<MatchDto?> GetByIdAsync(Guid id)
        {
            var match = await _unitOfWork.Matchs.GetMatchByIdAsync(id);
            return match is null ? null : MapToDto(match);
        }

        public async Task<MatchDto> CreateAsync(CreateMatchRequestDto dto, string? createdBy)
        {
            var match = new Match
            {
                Title = dto.Title,
                HomeTeam = dto.HomeTeam,
                AwayTeam = dto.AwayTeam,
                MatchDateTime = dto.MatchDateTime,
                Stadium = dto.Stadium,
                Location = dto.Location,
                TournamentStage = dto.TournamentStage,
                Status = dto.Status,
                PointsForCorrectPrediction = dto.PointsForCorrectPrediction,
                CreatedBy = createdBy
            };

            await _unitOfWork.Matchs.CreateMatchAsync(match);
            _unitOfWork.Complete();

            return MapToDto(match);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateMatchRequestDto dto, string? updatedBy)
        {
            var match = await _unitOfWork.Matchs.GetMatchByIdAsync(id);
            if (match is null)
            {
                return false;
            }

            match.Title = dto.Title;
            match.HomeTeam = dto.HomeTeam;
            match.AwayTeam = dto.AwayTeam;
            match.HomeTeamScore = dto.HomeTeamScore;
            match.AwayTeamScore = dto.AwayTeamScore;
            match.MatchDateTime = dto.MatchDateTime;
            match.Stadium = dto.Stadium;
            match.Location = dto.Location;
            match.TournamentStage = dto.TournamentStage;
            match.Status = dto.Status;
            match.TotalPredictions = dto.TotalPredictions;
            match.PointsForCorrectPrediction = dto.PointsForCorrectPrediction;
            match.UpdatedAt = DateTime.UtcNow;
            match.UpdatedBy = updatedBy;

            await _unitOfWork.Matchs.UpdateMatchAsync(match);
            _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var match = await _unitOfWork.Matchs.GetMatchByIdAsync(id);
            if (match is null)
            {
                return false;
            }

            await _unitOfWork.Matchs.DeleteMatchAsync(id);
            _unitOfWork.Complete();
            return true;
        }

        private static MatchDto MapToDto(Match match)
        {
            return new MatchDto
            {
                Id = match.Id,
                Title = match.Title,
                HomeTeam = match.HomeTeam,
                AwayTeam = match.AwayTeam,
                HomeTeamScore = match.HomeTeamScore,
                AwayTeamScore = match.AwayTeamScore,
                MatchDateTime = match.MatchDateTime,
                Stadium = match.Stadium,
                Location = match.Location,
                TournamentStage = match.TournamentStage,
                Status = match.Status,
                TotalPredictions = match.TotalPredictions,
                PointsForCorrectPrediction = match.PointsForCorrectPrediction
            };
        }
    }
}
