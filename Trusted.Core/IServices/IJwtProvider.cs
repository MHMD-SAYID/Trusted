using Trusted.Core.Entities;

namespace Trusted.Core.IServices;

public interface IJwtProvider
{
    string GenerateToken(User user, IEnumerable<string> roles);
}
