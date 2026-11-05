using System;
using System.Collections.Generic;
using System.Text;
using Trusted.ViewModel.DTO.Request.Images;

namespace Trusted.ViewModel.DTO.Request.Account
{
    public class CreateAccountReq
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
        public long GameId { get; set; }
        public List<long> MediatorIds { get; set; } = new List<long>();
        public List<UploadAccountImagesReq> Images { get; set; }
    }
}
