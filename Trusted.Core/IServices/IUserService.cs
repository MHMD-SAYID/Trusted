using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.ViewModel.DTO.Request.User;
using Trusted.ViewModel.DTO.Response.User;

namespace Trusted.Core.IServices
{
    public interface IUserService
    {
        Task<Result<UserProfileRes>> GetUserProfileAsync(long id);
        Task<Result<UserProfileRes>> UpdateUserProfileAsync(long id, UpdateUserProfileReq request);
        Task<Result<bool>> ChangePasswordAsync(long userId, ChangePasswordReq request);
    }
}
