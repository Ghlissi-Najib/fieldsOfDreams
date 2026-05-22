using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Domain.Entities
{
    public class AppNotification : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; } = NotificationType.Info;
        public bool IsRead { get; set; } = false;
        public string? RelatedEntityId { get; set; }
        public string? RelatedEntityType { get; set; }
    }
    public enum NotificationType
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 3
    }
}
