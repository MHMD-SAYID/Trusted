using System;
using System.Collections.Generic;
using System.Text;
using Trusted.ViewModel.DTO.Response.Account;

namespace Trusted.ViewModel.DTO.Response.User
{
    public class UserProfileRes
    {
        public string Name { get; set; }
        public string ProfilePic { get; set; }
        public int AccountsSold { get; set; }
        public int AccountsBought { get; set; }
        public double Rating { get; set; }
        public ICollection<MyListing> MyListing { get; set; }
    }
    public class MyListing
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsSold { get; set; }
        public string Game { get; set; }
    }

}
