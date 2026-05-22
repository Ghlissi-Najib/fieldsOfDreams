namespace Thefieldofdreams.Domain.Entities
{
    
    public class UserMission : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid MissionId { get; set; }
        public int CurrentProgress { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public int PointsAwarded { get; set; } = 0;

        public virtual User User { get; set; }
        public virtual Mission Mission { get; set; }
    }
}