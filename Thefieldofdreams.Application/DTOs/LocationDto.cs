using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public LocationType Type { get; set; }
        public string? ImageUrl { get; set; }
        public int PointsBonus { get; set; }
        public bool IsTourismSpot { get; set; }
    }

    public class CreateLocationRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        public LocationType Type { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsBonus { get; set; } = 0;

        public bool IsTourismSpot { get; set; } = false;
    }

    public class UpdateLocationRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        public LocationType Type { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int PointsBonus { get; set; } = 0;

        public bool IsTourismSpot { get; set; } = false;
    }
}
