using System.Security.Claims;

namespace Trusted.API.Extensions
{
    public static class UserExtensions
    {
        public static long? GetUserId(this ClaimsPrincipal user)
        {
            var idValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (long.TryParse(idValue, out var id))
                return id;
            return null;
        }
        public static string? GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role);
        }
    }
}
