using Trusted.Core.Entities.Games;
using Trusted.Core.IRepositories;

namespace Trusted.Infrastructure.Repositories
{
    public class DealRepository : BaseRepository<Deal>, IDealRepository
    {
        public DealRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

