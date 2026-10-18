using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.ViewModel.DTO.Response.Lookups;

namespace Trusted.Core.IServices
{
    public interface IGameService
    {
        Task<Result<List<GamelookupRes>>> GetAllAsync();
        Task<Result<List<GamelookupRes>>> GetPopularAsync();
    }
}
