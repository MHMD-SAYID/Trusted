using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities.Games;
using Trusted.Core.IRepositories;

namespace Trusted.Infrastructure.Repositories
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
