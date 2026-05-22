namespace Thefieldofdreams.Domain.Entities
{
    

    public class Sponsor : BaseEntity
    {
        public required string CompanyName { get; set; }
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public string? ContactEmail { get; set; }
        public string? ApiKey { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal? Budget { get; set; }
        public int TotalCampaigns { get; set; } = 0;

        public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
        public virtual ICollection<QRCode> QRCodes { get; set; } = new List<QRCode>();
        public virtual User? UserAccount { get; set; }
    }
}