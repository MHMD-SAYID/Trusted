using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities.Games;
using Trusted.Core.IRepositories;
using Trusted.ViewModel.DTO.Request.Account;

namespace Trusted.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Account> GetAllAccounts(AccountFilterReq filter, bool showAll)
        {
            var query = _context.Accounts.Where(a => a.Islive)
                .Include(a => a.Game)
                .Include(a => a.Images)
                .AsQueryable();

            // Adjust once you define what "showAll" hides for accounts
            // e.g. if (!showAll) query = query.Where(a => a.IsActive);

            if (filter.GameId.HasValue)
                query = query.Where(a => a.GameId == filter.GameId.Value);

            if (filter.MinPrice.HasValue)
                query = query.Where(a => a.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(a => a.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(a => a.Name.Contains(filter.Search));

            return query.OrderByDescending(a => a.CreatedAt);
        }

        public async Task<Account?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts
                .Include(a => a.Game)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            await _context.Accounts.AddAsync(account, cancellationToken);
        }

        public void Update(Account account)
        {
            _context.Accounts.Update(account);
        }

        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
