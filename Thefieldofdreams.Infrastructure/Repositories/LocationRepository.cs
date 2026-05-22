using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Location> GetLocationByIdAsync(Guid locationId)
        {
            return await _context.Locations.FirstOrDefaultAsync(l => l.Id == locationId);
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task CreateLocationAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

        public async Task UpdateLocationAsync(Location location)
        {
            _context.Locations.Update(location);
            await Task.CompletedTask;
        }

        public async Task DeleteLocationAsync(Guid locationId)
        {
            var location = await GetLocationByIdAsync(locationId);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }
            await Task.CompletedTask;
        }
    }
}