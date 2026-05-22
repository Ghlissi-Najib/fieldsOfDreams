using System.ComponentModel.DataAnnotations;

namespace Thefieldofdreams.Application.DTOs
{
    public class QRScanDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid QRCodeId { get; set; }
        public int PointsAwarded { get; set; }
        public string? LocationData { get; set; }
        public string? DeviceInfo { get; set; }
        public DateTime ScannedAt { get; set; }
    }

    public class CreateQRScanRequestDto
    {
        [Required]
        public Guid QRCodeId { get; set; }

        [StringLength(500)]
        public string? LocationData { get; set; }

        [StringLength(200)]
        public string? DeviceInfo { get; set; }
    }
}
