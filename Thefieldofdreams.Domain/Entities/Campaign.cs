namespace Thefieldofdreams.Domain.Entities
{
    
    public class Campaign : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid SponsorId { get; set; }
        public CampaignType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? Budget { get; set; }
        public int TotalImpressions { get; set; } = 0;
        public int TotalClicks { get; set; } = 0;
        public int TotalConversions { get; set; } = 0;
        public CampaignStatus Status { get; set; } = CampaignStatus.Draft;
        public int Priority { get; set; } = 0;

        public virtual Sponsor Sponsor { get; set; }
        public virtual ICollection<QRCampaign> QRCampaigns { get; set; } = new List<QRCampaign>();
        public virtual ICollection<CampaignRule> Rules { get; set; } = new List<CampaignRule>();
    }

    public enum CampaignType
    {
        QRBased,
        PredictionBased,
        ReferralBased,
        TourismBased,
        Hybrid
    }

    public enum CampaignStatus
    {
        Draft,
        Scheduled,
        Active,
        Paused,
        Completed,
        Cancelled
    }
}