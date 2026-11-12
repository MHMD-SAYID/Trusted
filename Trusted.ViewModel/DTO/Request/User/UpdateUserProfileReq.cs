using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.ViewModel.DTO.Request.User
{
    public class UpdateUserProfileReq
    {
        public string? Name { get; set; }
        public IFormFile? ProfilePic { get; set; }
    }
}
