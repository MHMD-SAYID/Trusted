using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Abstraction;
using Trusted.Core.Entities;
using Trusted.ViewModel.DTO.Response.Lookups;

namespace Trusted.Core.IServices
{
    public interface ILookupService
    {

        Task<Result<MediatorLookupRes>> GetAllMediators();
        
    }
}
