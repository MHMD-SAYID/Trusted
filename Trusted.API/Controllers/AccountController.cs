using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trusted.API.Extensions;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Request.Account;
using Trusted.ViewModel.DTO.Request.Common;

namespace Trusted.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet("")]
        public async Task<IActionResult> GEtAllAccounts([FromQuery] PaginationRequest request,
        [FromQuery] AccountFilterReq filter = null,
        CancellationToken cancellationToken = default)
        {
            long userId= User.GetUserId()??default(long);
            var userRole= User.GetUserRole()??default(string);
            var accounts =await _accountService.GetAllAccountsPaginated(userRole, request.PageNumber, request.PageSize, filter, userId, cancellationToken);
            return StatusCode(accounts.StatusCode, accounts);
        }
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccountById([FromRoute] long accountId)
        {
            var account =await _accountService.GetAccountDetailsAsync(accountId);
            return StatusCode(account.StatusCode, account);
        }
        [HttpPost("")]
        public async Task<IActionResult> CreateAccount([FromForm] CreateAccountReq req, CancellationToken cancellationToken = default)
        {
            long userId = User.GetUserId() ?? default(long);
            var result = await _accountService.CreateAccountAsync(req, userId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
