using System.ComponentModel.DataAnnotations;

namespace Thefieldofdreams.Application.DTOs
{
    public class UserMissionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MissionId { get; set; }
        public int CurrentProgress { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int PointsAwarded { get; set; }
    }

    public class CreateUserMissionRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid MissionId { get; set; }
    }

    public class UpdateUserMissionProgressDto
    {
        [Range(0, int.MaxValue)]
        public int CurrentProgress { get; set; }

        public bool IsCompleted { get; set; }
        public int PointsAwarded { get; set; }
    }
}
