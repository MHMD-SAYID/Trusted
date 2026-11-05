using Trusted.Core.Enums;

namespace Trusted.Core.Entities.Games
{
    public class Deal
    {
        public long Id { get; set; }

        public long AccountId { get; set; }
        public Account Account { get; set; }

        public long BuyerId { get; set; }
        public User Buyer { get; set; }

        public long SelectedMediatorId { get; set; }
        public User SelectedMediator { get; set; }

        public DealStatus Status { get; set; } = DealStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

