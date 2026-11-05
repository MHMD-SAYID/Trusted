using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trusted.Core.Abstraction;
using Trusted.Core.Constants;
using Trusted.Core.Entities;
using Trusted.Core.IServices;
using Trusted.ViewModel.DTO.Response.Lookups;

namespace Trusted.Application.Services
{
    public class LookupService : ILookupService
    {
        private readonly UserManager<User> _userManager;

        public LookupService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<MediatorLookupRes>> GetAllMediators()
        {
            try
            {
                var mediators = await _userManager.GetUsersInRoleAsync(SystemRoles.Mediator);

                var response = new MediatorLookupRes
                {
                    Mediators = mediators.Select(m => new Mediator
                    {
                        Id = m.Id,
                        Name = m.UserName ?? string.Empty,
                        Email = m.Email ?? string.Empty
                    }).ToList()
                };

                return new Result<MediatorLookupRes>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    ArRes = "تم جلب الوسطاء بنجاح",
                    EnRes = "Mediators fetched successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new Result<MediatorLookupRes>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    ArRes = "مشكلة في الخدمة",
                    EnRes = "Service error",
                    Data = null!,
                    Errors = new List<string> { ex.Message, ex.InnerException?.Message ?? "Nothing" }
                };
            }
        }
    }
}
