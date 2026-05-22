using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Thefieldofdreams.Domain.Entities
{
    // Domain/Entities/User.cs

    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public UserStatus Status { get; set; } = UserStatus.Active;
        public int TotalPoints { get; set; } = 0;
        public int CurrentLevel { get; set; } = 1;
        public DateTime? LastActiveAt { get; set; }
        public string? ReferralCode { get; set; }
        public Guid? ReferredByUserId { get; set; }

        // Navigation properties
        public virtual ICollection<Prediction> Predictions { get; set; } = new List<Prediction>();
        public virtual ICollection<Reward> Rewards { get; set; } = new List<Reward>();
        public virtual ICollection<QRScan> QRScans { get; set; } = new List<QRScan>();
        public virtual ICollection<UserMission> UserMissions { get; set; } = new List<UserMission>();
        public virtual Wallet Wallet { get; set; }
        public virtual ICollection<User> ReferredUsers { get; set; }
        public virtual User Referrer { get; set; }
    }

    public enum UserRole
    {
        User,
        VIP,
        Sponsor,
        Admin,
        SuperAdmin
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended,
        Banned
    }
}
