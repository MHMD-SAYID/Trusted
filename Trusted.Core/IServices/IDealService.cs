using Trusted.Core.Abstraction;
using Trusted.ViewModel.DTO.Request.Deal;
using Trusted.ViewModel.DTO.Response.Deal;

namespace Trusted.Core.IServices
{
    public interface IDealService
    {
        Task<Result<CreateDealRes>> CreateDealAsync(CreateDealReq req, long buyerId, CancellationToken cancellationToken = default);
        Task<Result<DealDetailsRes>> GetDealByIdAsync(long dealId, CancellationToken cancellationToken = default);
    }
}

