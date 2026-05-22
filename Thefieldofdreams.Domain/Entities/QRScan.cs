namespace Thefieldofdreams.Domain.Entities
{
    
    public class QRScan : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid QRCodeId { get; set; }
        public int PointsAwarded { get; set; }
        public string? LocationData { get; set; }
        public string? DeviceInfo { get; set; }
        public DateTime ScannedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
        public virtual QRCode QRCode { get; set; }
    }
}