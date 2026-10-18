using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.ViewModel.DTO.Request.Account
{
    public class AccountFilterReq
    {
        public long? GameId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Search { get; set; }
    }
}
