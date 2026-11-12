using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Trusted.Core.Abstraction;
using Trusted.Core.Entities;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Request.User;
using Trusted.ViewModel.DTO.Response.Account;
using Trusted.ViewModel.DTO.Response.User;

namespace Trusted.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly Regex PasswordRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#!]).{8,}$");

        public UserService(UserManager<User> userManager, IImageService imageService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<UserProfileRes>> GetUserProfileAsync(long id)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                return new Result<UserProfileRes>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ArRes = "المستخدم غير موجود",
                    EnRes = "User not found",
                    Data = null
                };
            }
            var myListing = await _userManager.Users
                .Where(u => u.Id == id)
                .SelectMany(u => u.Accounts)
                .Select(a => new MyListing
                {
                    Id = a.Id,
                    Name = a.Name,
                    Price = a.Price,
                    IsActive = a.Islive,
                    IsSold = a.IsSold,
                    Image = a.Images.FirstOrDefault(i=>i.IsDefault).Path,
                    Game = a.Game.EnName
                })
                .ToListAsync();
            var response = new UserProfileRes
            {
                Name = $"{user.FirstName} {user.LastName}".Trim(),
                ProfilePic = $"{baseUrl}{user.ProfilePic}",
                AccountsSold = user.AccountsSold,
                AccountsBought = user.AccountsBought,
                Rating = user.Rating,
                MyListing = myListing 
            };
            return new Result<UserProfileRes>
            {
                IsSuccess = true,
                StatusCode = 200,
                ArRes = "تم استرجاع الملف الشخصي بنجاح",
                EnRes = "Profile retrieved successfully",
                Data = response
            };
        }

        public async Task<Result<UserProfileRes>> UpdateUserProfileAsync(long id, UpdateUserProfileReq request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return new Result<UserProfileRes>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ArRes = "المستخدم غير موجود",
                    EnRes = "User not found",
                    Data = null
                };
            }

            // Update Name if provided
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var parts = request.Name.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                user.FirstName = parts[0];
                user.LastName = parts.Length > 1 ? parts[1] : string.Empty;
            }

            // Update ProfilePic if provided
            if (request.ProfilePic != null && request.ProfilePic.Length > 0)
            {
                var uploadResult = await _imageService.UploadProfilePicAsync(request.ProfilePic, id);
                if (!uploadResult.IsSuccess)
                {
                    return new Result<UserProfileRes>
                    {
                        IsSuccess = false,
                        StatusCode = uploadResult.StatusCode,
                        ArRes = uploadResult.ArRes,
                        EnRes = uploadResult.EnRes,
                        Data = null,
                        Errors = uploadResult.Errors
                    };
                }

                user.ProfilePic = uploadResult.Data;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new Result<UserProfileRes>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    ArRes = "فشل في تحديث الملف الشخصي",
                    EnRes = "Failed to update profile",
                    Data = null,
                    Errors = updateResult.Errors.Select(e => e.Description).ToList()
                };
            }

            var profileResult = await GetUserProfileAsync(id);
            if (profileResult.IsSuccess)
            {
                profileResult.ArRes = "تم تحديث الملف الشخصي بنجاح";
                profileResult.EnRes = "Profile updated successfully";
            }

            return profileResult;
        }

        public async Task<Result<bool>> ChangePasswordAsync(long userId, ChangePasswordReq request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    ArRes = "المستخدم غير موجود",
                    EnRes = "User not found",
                    Data = false
                };
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    ArRes = "كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين.",
                    EnRes = "New password and confirmation password do not match.",
                    Data = false,
                    Errors = new List<string> { "Passwords do not match." }
                };
            }

            if (request.CurrentPassword == request.NewPassword)
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    ArRes = "لا يمكن أن تكون كلمة المرور الجديدة مماثلة لكلمة المرور الحالية.",
                    EnRes = "New password cannot be the same as the current password.",
                    Data = false,
                    Errors = new List<string> { "New password cannot be identical to current password." }
                };
            }

            if (!PasswordRegex.IsMatch(request.NewPassword))
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    ArRes = "كلمة المرور يجب أن تحتوي على 8 أحرف على الأقل، وحرف كبير وحرف صغير ورقم ورمز خاص (@#!).",
                    EnRes = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character (@#!).",
                    Data = false,
                    Errors = new List<string> { "Password does not meet complexity requirements." }
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var isPasswordMismatch = result.Errors.Any(e => e.Code == "PasswordMismatch");
                return new Result<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    ArRes = isPasswordMismatch ? "كلمة المرور الحالية غير صحيحة." : "فشل في تغيير كلمة المرور.",
                    EnRes = isPasswordMismatch ? "Incorrect current password." : "Failed to change password.",
                    Data = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            return new Result<bool>
            {
                IsSuccess = true,
                StatusCode = 200,
                ArRes = "تم تغيير كلمة المرور بنجاح.",
                EnRes = "Password changed successfully.",
                Data = true
            };
        }
    }
}
