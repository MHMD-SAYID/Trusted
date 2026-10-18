using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities.Games;
using Trusted.ViewModel.DTO.Request.Account;

namespace Trusted.Core.IRepositories
{
    public interface IAccountRepository:IBaseRepository<Account>
    {
        IQueryable<Account> GetAllAccounts(AccountFilterReq filter, bool showAll);
        Task<Account?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task AddAsync(Account account, CancellationToken cancellationToken = default);
        void Update(Account account);
        void Delete(Account account);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
