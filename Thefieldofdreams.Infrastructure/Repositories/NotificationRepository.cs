using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppNotification> GetByIdAsync(Guid id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<AppNotification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<IEnumerable<AppNotification>> FindAsync(System.Linq.Expressions.Expression<Func<AppNotification, bool>> predicate)
        {
            return await _context.Notifications.Where(predicate).ToListAsync();
        }

        public async Task<AppNotification> AddAsync(AppNotification entity)
        {
            await _context.Notifications.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(AppNotification entity)
        {
            _context.Notifications.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
            }
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Notifications.AnyAsync(n => n.Id == id);
        }

        public async Task<IEnumerable<AppNotification>> GetForUserAsync(string userId, DateTime since)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && n.CreatedAt >= since)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(Guid id, string userId)
        {
            var notification = await GetByIdAsync(id);
            if (notification != null && notification.UserId == userId)
            {
                notification.IsRead = true;
                _context.Notifications.Update(notification);
            }
            await Task.CompletedTask;
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            _context.Notifications.UpdateRange(notifications);
            await Task.CompletedTask;
        }
    }
}