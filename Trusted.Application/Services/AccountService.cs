using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.Core.Constants;
using Trusted.Core.Entities.Games;
using Trusted.Core.IRepositories;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Request.Account;
using Trusted.ViewModel.DTO.Response.Account;

namespace Trusted.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IAccountRepository accountRepository, IGameRepository gameRepository, IImageService imageService, IHttpContextAccessor httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _gameRepository = gameRepository;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<CreateAccountRes>> CreateAccountAsync(CreateAccountReq req, long UserId, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _accountRepository.BeginTransactionAsync();
            try
            {
                if (req.Images.Count < 1)
                {

                    return new Result<CreateAccountRes>
                    {
                        IsSuccess = false,
                        EnRes = "At least 1 images are required",
                        ArRes = "مطلوب 1 صور على الأقل",
                        StatusCode = 400
                    };
                }
                if (req.Images.Count > 3)
                {
                    return new Result<CreateAccountRes>
                    {
                        IsSuccess = false,
                        EnRes = "A maximum of 3 images are allowed",
                        ArRes = "يسمح بحد أقصى 3 صور",
                        StatusCode = 400
                    };
                }

                if (!await _gameRepository.ExistsAsync(g => g.Id == req.GameId))
                {
                    return new Result<CreateAccountRes>
                    {
                        IsSuccess = false,
                        EnRes = "Game not found",
                        ArRes = "اللعبة غير موجودة",
                        StatusCode = 404
                    };
                }

                var accont = new Account
                {
                    Name = req.Name,
                    UserId = UserId,
                    GameId = req.GameId,
                    Price = req.Price,
                    Details = req.Details,
                    CreatedAt = DateTime.UtcNow,
                    Islive=true
                };
                await _accountRepository.AddAsync(accont);
                await _accountRepository.CommitAsync();

                var uploadedImages = _imageService.UploadManyAsync(req.Images, accont.Id, cancellationToken);
                if (!uploadedImages.Result.IsSuccess)
                {
                    transaction.Rollback();
                    return new Result<CreateAccountRes>
                    {
                        IsSuccess = false,
                        EnRes = "Failed to upload images",
                        ArRes = "فشل في تحميل الصور",
                        StatusCode = 500
                    };
                }
                await _accountRepository.CommitTransactionAsync(transaction);
                return new Result<CreateAccountRes>
                {
                    IsSuccess = true,
                    EnRes = "Account created successfully",
                    ArRes = "تم إنشاء الحساب بنجاح",
                    StatusCode = 201,
                    Data = new CreateAccountRes { AccountId = accont.Id }
                };
            }
            catch (Exception ex)
            {
                await _accountRepository.RollbackTransactionAsync(transaction);

                return new Result<CreateAccountRes>
                {

                    IsSuccess = false,
                    ArRes = "حدث خطأ أثناء إنشاء الحساب.",
                    EnRes = "An error occurred while creating the account.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message,ex.StackTrace,ex.InnerException.Message }
                };
            }
        }

        private IQueryable<GetPaginatedAccountsRes> GetAllAccounts(bool showAll, AccountFilterReq filter, CancellationToken cancellationToken = default)
        {
            var accountsQuery = _accountRepository.GetAllAccounts(filter, showAll);

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var response = accountsQuery.Select(a => new GetPaginatedAccountsRes
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price,
                
                GameId = a.GameId,
                GameName = a.Game.EnName,
               
                ImageURL= a.Images.FirstOrDefault(img => img.IsDefault) != null
                    ? new AccountImageRes
                    {
                        Id = a.Images.FirstOrDefault(img => img.IsDefault).Id,
                        Image = $"{baseUrl}/{a.Images.FirstOrDefault(img => img.IsDefault).Path}",
                        IsDefault = true
                    }
                    : null,
            });

            return response;
        }

        public async Task<Result<PaginatedList<GetPaginatedAccountsRes>>> GetAllAccountsPaginated(
            string? userRole,
            int pageNumber,
            int pageSize,
            AccountFilterReq filter,
            long userId,
            CancellationToken cancellationToken)
        {
            try
            {
                var privilegedRoles = new[]
                {
            SystemRoles.User,

        };

                var showAll = userRole != null && privilegedRoles.Contains(userRole);

                var accountsQuery = GetAllAccounts(showAll, filter).AsQueryable();

                var response = await PaginatedList<GetPaginatedAccountsRes>.CreateAsync(accountsQuery, pageNumber, pageSize);

                return new Result<PaginatedList<GetPaginatedAccountsRes>>
                {
                    EnRes = "Accounts fetched successfully",
                    ArRes = "تم جلب الحسابات بنجاح",
                    Data = response,
                    StatusCode = 200,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Result<PaginatedList<GetPaginatedAccountsRes>>
                {
                    IsSuccess = false,
                    EnRes = "Service is down",
                    ArRes = "مشكله فى الخدمه",
                    StatusCode = 400,
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
            }
        }
        public async Task<Result<AccountDetailsRes>> GetAccountDetailsAsync(long accountId, CancellationToken cancellationToken = default)
        {
            try
            {
                var account = await _accountRepository.FindAsync(a => a.Id == accountId, ["Images", "Game", "User"]);
                if (account == null)
                {
                    return new Result<AccountDetailsRes>
                    {
                        IsSuccess = false,
                        EnRes = "Account not found",
                        ArRes = "الحساب غير موجود",
                        StatusCode = 404
                    };
                }
                var response = new AccountDetailsRes
                {
                    Id = account.Id,
                    Name = account.Name,
                    Price = account.Price,
                    Details = account.Details,
                    GameId = account.GameId,
                    GameName = account.Game.EnName,
                    GameLogo = account.Game.Logo,
                    CreatedAt = account.CreatedAt,
                    UserName = account.User.UserName,
                    ImagesURL = account.Images.Select(img => new AccountImageRes
                    {
                        Id = img.Id,
                        Image = img.Path,
                        IsDefault = img.IsDefault
                    }).ToList(),
                    IsFeatured = account.IsFeatured
                };
                return new Result<AccountDetailsRes>
                {
                    IsSuccess = true,
                    EnRes = "Account details fetched successfully",
                    ArRes = "تم جلب تفاصيل الحساب بنجاح",
                    StatusCode = 200,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new Result<AccountDetailsRes>
                {
                    IsSuccess = false,
                    EnRes = "Service is down",
                    ArRes = "مشكله فى الخدمه",
                    StatusCode = 400,
                    Errors = new List<string> { ex.Message, ex.StackTrace, ex.InnerException?.Message }
                };

            }
        }
    }
}
