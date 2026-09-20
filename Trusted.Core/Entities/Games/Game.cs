using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.Core.Entities.Games
{
    public class Game
    {
        public long Id { get; set; }
        
        public string ArName { get; set; }
        public string EnName { get; set; }
        public string Logo { get; set; }

    }
}
