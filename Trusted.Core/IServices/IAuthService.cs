using Trusted.Core.Abstraction;
using Trusted.ViewModel.DTO.Request.Auth;
using Trusted.ViewModel.DTO.Response.Auth;

namespace Trusted.Core.IServices;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
}
