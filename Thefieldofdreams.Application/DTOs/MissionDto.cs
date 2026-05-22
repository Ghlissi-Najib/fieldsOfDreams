using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class MissionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public MissionType Type { get; set; }
        public int RequiredProgress { get; set; }
        public int PointsReward { get; set; }
        public int? XPValue { get; set; }
        public MissionDifficulty Difficulty { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDaily { get; set; }
        public int? RequiredUserLevel { get; set; }
    }

    public class CreateMissionRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public MissionType Type { get; set; }

        [Range(1, int.MaxValue)]
        public int RequiredProgress { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int PointsReward { get; set; }

        public int? XPValue { get; set; }

        public MissionDifficulty Difficulty { get; set; } = MissionDifficulty.Medium;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDaily { get; set; } = false;
        public int? RequiredUserLevel { get; set; }
    }

    public class UpdateMissionRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public MissionType Type { get; set; }

        [Range(1, int.MaxValue)]
        public int RequiredProgress { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int PointsReward { get; set; }

        public int? XPValue { get; set; }

        public MissionDifficulty Difficulty { get; set; } = MissionDifficulty.Medium;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDaily { get; set; } = false;
        public int? RequiredUserLevel { get; set; }
    }
}
