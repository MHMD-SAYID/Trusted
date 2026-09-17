using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Trusted.Core;
using Trusted.Core.Abstraction;
using Trusted.Core.Entities;
using Trusted.Core.IServices;
using Trusted.ViewModel;
using Trusted.ViewModel.Auth;

namespace Trusted.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly JwtOptions _jwtOptions;
    
    public AuthService(
        UserManager<User> userManager,
        IJwtProvider jwtProvider,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {

                return new Result<AuthResponse>()
                {
                    ArRes = "يوجد مستخدم بهذا البريد الإلكتروني بالفعل.",
                    EnRes = "A user with this email already exists.",
                    StatusCode = 400,
                    Data = null,
                    IsSuccess = false
                };
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new Result<AuthResponse>
                {
                    ArRes = "فشل التسجيل.",
                    EnRes = "Registration failed.",
                    StatusCode = 400,
                    Data = null,
                    IsSuccess = false,
                    Errors = errors

                };
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "User");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtProvider.GenerateToken(user, roles);

            var response = new AuthResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
                Email = user.Email!,
                Roles = roles
            };

            return new Result<AuthResponse>()
            {
                IsSuccess = true,
                ArRes = "تم التسجيل بنجاح.",
                EnRes = "Registration successful.",
                StatusCode = 200,
                Data = response
            };
        }
        catch (Exception ex)
        {
            return new Result<AuthResponse>()
            {
                IsSuccess = false,
                ArRes = "حدث خطأ أثناء التسجيل.",
                EnRes = "An error occurred during registration.",
                StatusCode = 500,
                Data = null,
                Errors = new List<string> { ex.Message ,ex.InnerException?.Message??"Nothing" }
            };
        }
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return new Result<AuthResponse>()
            {
                IsSuccess = false,
                ArRes = "البريد الإلكتروني أو كلمة المرور غير صحيحة.",
                EnRes = "Invalid email or password.",
                StatusCode = 400,
                Data = null
            };
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return new Result<AuthResponse>()
            {
                IsSuccess = false,
                ArRes = "البريد الإلكتروني أو كلمة المرور غير صحيحة.",
                EnRes = "Invalid email or password.",
                StatusCode = 400,
                Data = null
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtProvider.GenerateToken(user, roles);

        var response = new AuthResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
            Email = user.Email!,
            Roles = roles
        };
        return new Result<AuthResponse>()
        {
            IsSuccess = true,
            ArRes = "تم تسجيل الدخول بنجاح.",
            EnRes = "Login successful.",
            StatusCode = 200,
            Data = response
        };
    }
}
