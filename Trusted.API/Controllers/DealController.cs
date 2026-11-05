using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trusted.API.Extensions;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Request.Deal;

namespace Trusted.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DealController : ControllerBase
    {
        private readonly IDealService _dealService;

        public DealController(IDealService dealService)
        {
            _dealService = dealService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateDeal([FromBody] CreateDealReq req, CancellationToken cancellationToken = default)
        {
            long buyerId = User.GetUserId() ?? default;
            var result = await _dealService.CreateDealAsync(req, buyerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{dealId}")]
        public async Task<IActionResult> GetDealById([FromRoute] long dealId, CancellationToken cancellationToken = default)
        {
            var result = await _dealService.GetDealByIdAsync(dealId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

