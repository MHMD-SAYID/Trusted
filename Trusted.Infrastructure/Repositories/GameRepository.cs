using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities.Games;
using Trusted.Core.IRepositories;

namespace Trusted.Infrastructure.Repositories
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        public GameRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
