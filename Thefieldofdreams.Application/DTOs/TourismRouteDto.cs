using System.ComponentModel.DataAnnotations;

namespace Thefieldofdreams.Application.DTOs
{
    public class TourismRouteDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalPointsReward { get; set; }
        public int EstimatedDurationMinutes { get; set; }
        public bool IsHiddenGemRoute { get; set; }
    }

    public class CreateTourismRouteRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalPointsReward { get; set; }

        [Range(1, int.MaxValue)]
        public int EstimatedDurationMinutes { get; set; }

        public bool IsHiddenGemRoute { get; set; } = false;
    }

    public class UpdateTourismRouteRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalPointsReward { get; set; }

        [Range(1, int.MaxValue)]
        public int EstimatedDurationMinutes { get; set; }

        public bool IsHiddenGemRoute { get; set; } = false;
    }
}
