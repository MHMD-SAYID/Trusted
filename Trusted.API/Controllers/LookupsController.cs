using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trusted.Core.IServices;

namespace Trusted.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILookupService _lookupService;

        public LookupsController(IGameService gameService, ILookupService lookupService)
        {
            _gameService = gameService;
            _lookupService = lookupService;
        }
        [HttpGet("games")]
        public async Task<IActionResult> GetAllGames()
        {
            var result = await _gameService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("games/popular")]
        public async Task<IActionResult> GetPopularGames()
        {
            var result = await _gameService.GetPopularAsync();
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("mediators")]
        public async Task<IActionResult> GetAllMediators()
        {
            var result = await _lookupService.GetAllMediators();
            return StatusCode(result.StatusCode, result);
        }
    }
}
