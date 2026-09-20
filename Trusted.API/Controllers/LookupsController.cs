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

        public LookupsController(IGameService gameService)
        {
            _gameService = gameService;
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
    }
}
