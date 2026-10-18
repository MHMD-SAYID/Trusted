using System;
using System.Collections.Generic;
using System.Text;

namespace Trusted.ViewModel.DTO.Response.Account
{
    public class GetPaginatedAccountsRes
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        //public string Details { get; set; }
        public long GameId { get; set; }
        public string GameName { get; set; }
        //public string GameLogo { get; set; }
        //public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public AccountImageRes ImageURL { get; set; }
        public bool IsFeatured { get; set; }
    }
}
