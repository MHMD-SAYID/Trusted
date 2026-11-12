using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trusted.API.Extensions;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Request.User;

namespace Trusted.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.GetUserId() ?? default(long);
            var result = await _userService.GetUserProfileAsync(userId);
           
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileReq request)
        {
            var userId = User.GetUserId() ?? default(long);
            var result = await _userService.UpdateUserProfileAsync(userId, request);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordReq request)
        {
            var userId = User.GetUserId() ?? default(long);
            var result = await _userService.ChangePasswordAsync(userId, request);

            return StatusCode(result.StatusCode, result);
        }
    }
}
