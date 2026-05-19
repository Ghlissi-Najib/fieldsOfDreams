using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Domain.Interfaces
{
    public interface ILocationRepository
    {
        Task<Location> GetLocationByIdAsync(Guid locationId);
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task CreateLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(Guid locationId);
    }
}