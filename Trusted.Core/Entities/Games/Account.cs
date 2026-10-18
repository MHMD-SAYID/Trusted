using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.Core.Entities.Games
{
    public class Account
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public User User { get; set; }
        public long UserId { get; set; }
        public Game Game { get; set; }
        public long GameId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Islive { get; set; }
        public bool IsFeatured { get; set; }
    }
}
