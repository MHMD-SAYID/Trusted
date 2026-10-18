using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.RegularExpressions;
using Trusted.Core;
using Trusted.Core.Abstraction;
using Trusted.Core.Entities;
using Trusted.Core.IServices;
using Trusted.ViewModel;
using Trusted.ViewModel.DTO.Request.Auth;
using Trusted.ViewModel.DTO.Response.Auth;

namespace Trusted.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly JwtOptions _jwtOptions;
    private readonly Regex _phoneNumberRegex = new Regex(@"^\+201[0125]\d{8}$");
    private readonly Regex passwordRegex = new Regex( @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#!]).{8,}$"
);

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
            var phoneExists = await _userManager.Users
    .AnyAsync(u => u.PhoneNumber == request.PhoneNumber);
            if(phoneExists)
            {
                return new Result<AuthResponse>()
                {
                    ArRes = "???? ?????? ? ??? ?????",
                    EnRes = "A user with this phone number already exists.",
                    StatusCode = 400,
                    Data = null,
                    IsSuccess = false
                };
            }
            if (Regex.IsMatch(request.PhoneNumber, _phoneNumberRegex.ToString()) == false)
            {
                return new Result<AuthResponse>()
                {
                    ArRes = "??? ?????? ??? ????.",
                    EnRes = "Invalid phone number.",
                    StatusCode = 400,
                    Data = null,
                    IsSuccess = false
                };
            }
            if(Regex.IsMatch(request.Password, passwordRegex.ToString()) == false)
            {
                return new Result<AuthResponse>()
                {
                    ArRes = "???? ?????? ??? ?? ????? ??? ??? ???? ????? ??? ???? ????? ??? ????? ???? ??? ????.",
                    EnRes = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.",
                    StatusCode = 400,
                    Data = null,
                    IsSuccess = false
                };
            }
            // to be changed => return error and log that the ip address is not valid
            var ipRegex = new Regex(
     @"^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)){3}$"
 );

            if (!ipRegex.IsMatch(request.IpAddress))
            {
                return new Result<AuthResponse>
                {
                    ArRes = "????? IP ??? ????.",
                    EnRes = "Invalid IP address.",
                    StatusCode = 400,
                    Data = null,
                    IsSuccess = false
                };
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber= request.PhoneNumber,
                PhoneNumberConfirmed = true,
                IpAddress = request.IpAddress,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return new Result<AuthResponse>
                {
                    ArRes = "??? ???????.",
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
                Role = roles.FirstOrDefault() ?? string.Empty
            };

            return new Result<AuthResponse>()
            {
                IsSuccess = true,
                ArRes = "?? ??????? ?????",
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
                ArRes = "????? ?? ??????",
                EnRes = "Service is down",
                Data = null,
                StatusCode = 500,
                Errors = new List<string> { ex.Message, ex.InnerException?.Message ?? "Nothing" }
            };
        }
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new Result<AuthResponse>()
                {
                    IsSuccess = false,
                    ArRes = "?????? ?????????? ?? ???? ?????? ??? ?????.",
                    EnRes = "Invalid email or password.",
                    StatusCode = 400,
                    Data = null
                };
            }
            if (user.IsActive == false)
            {
                return new Result<AuthResponse>()
                {
                    IsSuccess = false,
                    ArRes = "?????? ????. ???? ??????? ?? ???????.",
                    EnRes = "Your account has been disabled. Please contact support.",
                    StatusCode = 403,
                    Data = null
                };
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return new Result<AuthResponse>()
                {
                    IsSuccess = false,
                    ArRes = "?????? ?????????? ?? ???? ?????? ??? ?????.",
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
                Role = roles.FirstOrDefault() ?? string.Empty
            };
            return new Result<AuthResponse>()
            {
                IsSuccess = true,
                ArRes = "?? ??????? ?????",
                EnRes = "Login successful.",
                StatusCode = 200,
                Data = response
            };
        }
        catch (Exception ex)
        {
            return new Result<AuthResponse>()
            {
                IsSuccess = false,
                ArRes = "????? ?? ??????",
                EnRes = "Service is down",
                Data = null,
                StatusCode = 500,
                Errors = new List<string> { ex.Message, ex.InnerException?.Message ?? "Nothing" }
            };
        }
    }
}
