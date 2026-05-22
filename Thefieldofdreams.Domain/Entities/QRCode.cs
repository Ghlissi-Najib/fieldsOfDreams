using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Domain.Entities
{

    public class QRCode : BaseEntity
    {
        public required string Code { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public QRCodeType Type { get; set; }
        public Guid? SponsorId { get; set; }
        public Guid? LocationId { get; set; }
        public int PointsReward { get; set; } = 10;
        public int MaxScans { get; set; } = 0; // 0 = unlimited
        public int CurrentScanCount { get; set; } = 0;
        public DateTime? ExpiryDate { get; set; }
        public bool IsLimitedTime { get; set; } = false;

        public virtual Sponsor? Sponsor { get; set; }
        public virtual Location? Location { get; set; }
        public virtual ICollection<QRCampaign> QRCampaigns { get; set; }

        public virtual ICollection<QRScan> QRScans { get; set; } = new List<QRScan>();
    }

    public enum QRCodeType
    {
        Tourism,
        Sponsor,
        Mission,
        Reward,
        Checkpoint,
        HiddenGem
    }
}
