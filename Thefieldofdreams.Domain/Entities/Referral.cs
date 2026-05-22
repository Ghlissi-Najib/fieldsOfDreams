using System;
using System.Collections.Generic;
using System.Text;

namespace Thefieldofdreams.Domain.Entities
{
    public class Referral : BaseEntity
    {
        public Guid ReferrerUserId { get; set; }
        public Guid ReferredUserId { get; set; }
        public int PointsAwarded { get; set; }
        public bool IsCompleted { get; set; }
    }
}
