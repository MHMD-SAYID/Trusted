using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.Core.Entities.Games
{
    public class Image
    {
        public long Id { get; set; }
        public string  Path { get; set; }
        public bool IsDefault { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
    }
}
