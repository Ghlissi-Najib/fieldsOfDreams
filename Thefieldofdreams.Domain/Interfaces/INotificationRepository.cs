using System;
using System.Collections.Generic;
using System.Text;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Domain.Interfaces
{
    public interface INotificationRepository : IRepository<AppNotification>
    {
        Task<IEnumerable<AppNotification>> GetForUserAsync(string userId, DateTime since);
        Task MarkAsReadAsync(Guid id, string userId);
        Task MarkAllAsReadAsync(string userId);
        Task<AppNotification> AddAsync(AppNotification entity);
    }

}
