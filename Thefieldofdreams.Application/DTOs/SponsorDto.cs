using System.ComponentModel.DataAnnotations;

namespace Thefieldofdreams.Application.DTOs
{
    public class SponsorDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public string? ContactEmail { get; set; }
        public bool IsActive { get; set; }
        public decimal? Budget { get; set; }
        public int TotalCampaigns { get; set; }
    }

    public class CreateSponsorRequestDto
    {
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? LogoUrl { get; set; }

        [StringLength(300)]
        public string? Website { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? ContactEmail { get; set; }

        public bool IsActive { get; set; } = true;

        public decimal? Budget { get; set; }
    }

    public class UpdateSponsorRequestDto
    {
        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? LogoUrl { get; set; }

        [StringLength(300)]
        public string? Website { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? ContactEmail { get; set; }

        public bool IsActive { get; set; } = true;

        public decimal? Budget { get; set; }
    }
}
