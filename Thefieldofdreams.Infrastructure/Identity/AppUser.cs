using Microsoft.AspNetCore.Identity;
using Thefieldofdreams.Application.Security;

namespace Thefieldofdreams.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePhotoUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public UserRole Role { get; set; } = UserRole.Passenger;
        public Guid? MerchantId { get; set; }
        public Guid? PartnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Gamification
        public int TotalPoints { get; set; } = 0;
        public int CurrentLevel { get; set; } = 1;
        public int ExperiencePoints { get; set; } = 0;
        public int TicketCount { get; set; } = 0;

        // Referral System
        public string? ReferralCode { get; set; }
        public string? ReferredByUserId { get; set; }
        public int ReferralCount { get; set; } = 0;

        // Wallet
        public string? WalletId { get; set; }

        // Navigation property
        public virtual AppUser? ReferredBy { get; set; }
        public virtual ICollection<AppUser> Referrals { get; set; } = new List<AppUser>();
    }
}
