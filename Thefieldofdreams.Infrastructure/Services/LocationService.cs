using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LocationDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Locations.GetAllLocationsAsync();
            return items.Select(MapToDto);
        }

        public async Task<LocationDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Locations.GetLocationByIdAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<LocationDto> CreateAsync(CreateLocationRequestDto dto, string? createdBy)
        {
            var entity = new Location
            {
                Name = dto.Name,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                Type = dto.Type,
                ImageUrl = dto.ImageUrl,
                PointsBonus = dto.PointsBonus,
                IsTourismSpot = dto.IsTourismSpot,
                CreatedBy = createdBy
            };

            await _unitOfWork.Locations.CreateLocationAsync(entity);
            _unitOfWork.Complete();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateLocationRequestDto dto, string? updatedBy)
        {
            var entity = await _unitOfWork.Locations.GetLocationByIdAsync(id);
            if (entity is null) return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Country = dto.Country;
            entity.Type = dto.Type;
            entity.ImageUrl = dto.ImageUrl;
            entity.PointsBonus = dto.PointsBonus;
            entity.IsTourismSpot = dto.IsTourismSpot;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _unitOfWork.Locations.UpdateLocationAsync(entity);
            _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Locations.GetLocationByIdAsync(id);
            if (entity is null) return false;

            await _unitOfWork.Locations.DeleteLocationAsync(id);
            _unitOfWork.Complete();
            return true;
        }

        private static LocationDto MapToDto(Location l) => new()
        {
            Id = l.Id,
            Name = l.Name,
            Description = l.Description,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            Address = l.Address,
            City = l.City,
            Country = l.Country,
            Type = l.Type,
            ImageUrl = l.ImageUrl,
            PointsBonus = l.PointsBonus,
            IsTourismSpot = l.IsTourismSpot
        };
    }
}
