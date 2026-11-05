using Trusted.Core.Abstraction;
using Trusted.Core.Entities.Games;
using Trusted.Core.Enums;
using Trusted.Core.IRepositories;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Request.Deal;
using Trusted.ViewModel.DTO.Response.Deal;

namespace Trusted.Application.Services
{
    public class DealService : IDealService
    {
        private readonly IDealRepository _dealRepository;
        private readonly IAccountRepository _accountRepository;

        public DealService(IDealRepository dealRepository, IAccountRepository accountRepository)
        {
            _dealRepository = dealRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Result<CreateDealRes>> CreateDealAsync(CreateDealReq req, long buyerId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Load account with its candidate mediators
                var account = await _accountRepository.FindAsync(
                    a => a.Id == req.AccountId,
                    ["AccountMediators"]);

                if (account == null)
                    return new Result<CreateDealRes>
                    {
                        IsSuccess = false,
                        EnRes = "Account not found",
                        ArRes = "الحساب غير موجود",
                        StatusCode = 404
                    };

                if (!account.Islive)
                    return new Result<CreateDealRes>
                    {
                        IsSuccess = false,
                        EnRes = "This account is no longer available",
                        ArRes = "هذا الحساب لم يعد متاحًا",
                        StatusCode = 400
                    };

                // Buyer cannot purchase their own account
                if (account.UserId == buyerId)
                    return new Result<CreateDealRes>
                    {
                        IsSuccess = false,
                        EnRes = "You cannot buy your own account",
                        ArRes = "لا يمكنك شراء حسابك الخاص",
                        StatusCode = 400
                    };

                // Chosen mediator must be one of the 3 candidates on this account
                var validMediatorIds = account.AccountMediators.Select(am => am.MediatorId).ToHashSet();
                if (!validMediatorIds.Contains(req.SelectedMediatorId))
                    return new Result<CreateDealRes>
                    {
                        IsSuccess = false,
                        EnRes = "The selected mediator is not assigned to this account",
                        ArRes = "الوسيط المحدد غير مخصص لهذا الحساب",
                        StatusCode = 400
                    };

                // Prevent duplicate active deals for the same buyer on the same account
                var existingDeal = await _dealRepository.FindAsync(
                    d => d.AccountId == req.AccountId
                      && d.BuyerId == buyerId
                      && d.Status == DealStatus.Pending);

                if (existingDeal != null)
                    return new Result<CreateDealRes>
                    {
                        IsSuccess = false,
                        EnRes = "You already have a pending deal for this account",
                        ArRes = "لديك بالفعل صفقة معلقة لهذا الحساب",
                        StatusCode = 400
                    };

                var deal = new Deal
                {
                    AccountId = req.AccountId,
                    BuyerId = buyerId,
                    SelectedMediatorId = req.SelectedMediatorId,
                    Status = DealStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                await _dealRepository.AddAsync(deal);
                await _dealRepository.CommitAsync();

                return new Result<CreateDealRes>
                {
                    IsSuccess = true,
                    EnRes = "Deal created successfully",
                    ArRes = "تم إنشاء الصفقة بنجاح",
                    StatusCode = 201,
                    Data = new CreateDealRes { DealId = deal.Id }
                };
            }
            catch (Exception ex)
            {
                return new Result<CreateDealRes>
                {
                    IsSuccess = false,
                    EnRes = "Service is down",
                    ArRes = "مشكلة في الخدمة",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message, ex.StackTrace ?? "", ex.InnerException?.Message ?? "" }
                };
            }
        }

        public async Task<Result<DealDetailsRes>> GetDealByIdAsync(long dealId, CancellationToken cancellationToken = default)
        {
            try
            {
                var deal = await _dealRepository.FindAsync(
                    d => d.Id == dealId,
                    ["Account", "Buyer", "SelectedMediator"]);

                if (deal == null)
                    return new Result<DealDetailsRes>
                    {
                        IsSuccess = false,
                        EnRes = "Deal not found",
                        ArRes = "الصفقة غير موجودة",
                        StatusCode = 404
                    };

                return new Result<DealDetailsRes>
                {
                    IsSuccess = true,
                    EnRes = "Deal fetched successfully",
                    ArRes = "تم جلب الصفقة بنجاح",
                    StatusCode = 200,
                    Data = new DealDetailsRes
                    {
                        Id = deal.Id,
                        AccountId = deal.AccountId,
                        AccountName = deal.Account?.Name ?? string.Empty,
                        BuyerId = deal.BuyerId,
                        BuyerName = deal.Buyer?.UserName ?? string.Empty,
                        SelectedMediatorId = deal.SelectedMediatorId,
                        SelectedMediatorName = deal.SelectedMediator?.UserName ?? string.Empty,
                        Status = deal.Status.ToString(),
                        CreatedAt = deal.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new Result<DealDetailsRes>
                {
                    IsSuccess = false,
                    EnRes = "Service is down",
                    ArRes = "مشكلة في الخدمة",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message, ex.StackTrace ?? "", ex.InnerException?.Message ?? "" }
                };
            }
        }
    }
}

