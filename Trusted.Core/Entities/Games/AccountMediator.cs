using System;
using Trusted.Core.Entities;

namespace Trusted.Core.Entities.Games
{
    public class AccountMediator
    {
        public long AccountId { get; set; }
        public Account Account { get; set; }

        public long MediatorId { get; set; }
        public User Mediator { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}

