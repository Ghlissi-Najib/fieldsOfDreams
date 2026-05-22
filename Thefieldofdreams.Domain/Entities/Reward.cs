namespace Thefieldofdreams.Domain.Entities
{
 
    public class Reward : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int PointsRequired { get; set; }
        public RewardType Type { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; } = 0;
        public int ClaimedCount { get; set; } = 0;
        public DateTime? ExpiryDate { get; set; }
        public bool IsVIPOnly { get; set; } = false;

        public virtual ICollection<UserReward> UserRewards { get; set; } = new List<UserReward>();
    }

    public enum RewardType
    {
        DiscountCoupon,
        FreeDrink,
        Merchandise,
        TicketUpgrade,
        VIPAccess,
        DigitalBadge,
        Cashback
    }
}