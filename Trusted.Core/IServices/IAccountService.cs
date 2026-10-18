using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.Core.Constants;
using Trusted.ViewModel.DTO.Request.Account;
using Trusted.ViewModel.DTO.Response.Account;

namespace Trusted.Core.IServices
{
    public interface IAccountService
    {
        Task<Result<CreateAccountRes>> CreateAccountAsync(CreateAccountReq req,long UserId, CancellationToken cancellationToken = default);
        Task<Result<AccountDetailsRes>> GetAccountDetailsAsync(long accountId, CancellationToken cancellationToken = default);
        Task<Result<PaginatedList<GetPaginatedAccountsRes>>> GetAllAccountsPaginated(string? userRole,int pageNumber,int pageSize,AccountFilterReq filter,long userId,CancellationToken cancellationToken);
    }
}
