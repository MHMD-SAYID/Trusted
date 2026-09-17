using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities;

namespace Trusted.Core.Constants
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(User user, string role);
        string? GetRoleFromToken(string token);

        string? ValidateToken(string token);
    }
}
