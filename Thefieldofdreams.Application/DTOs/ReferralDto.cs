using System.ComponentModel.DataAnnotations;

namespace Thefieldofdreams.Application.DTOs
{
    public class ReferralDto
    {
        public Guid Id { get; set; }
        public Guid ReferrerUserId { get; set; }
        public Guid ReferredUserId { get; set; }
        public int PointsAwarded { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReferralRequestDto
    {
        [Required]
        public Guid ReferrerUserId { get; set; }

        [Required]
        public Guid ReferredUserId { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsAwarded { get; set; }
    }
}
