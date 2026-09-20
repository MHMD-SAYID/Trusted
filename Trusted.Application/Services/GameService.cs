using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Response.Lookups;

namespace Trusted.Application.Services
{
    public class GameService : IGameService
    {
        public Task<Result<GamelookupRes>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<GamelookupRes>> GetPopularAsync()
        {
            throw new NotImplementedException();
        }
    }
}
