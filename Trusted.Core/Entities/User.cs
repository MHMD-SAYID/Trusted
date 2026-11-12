using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Trusted.Core.Entities.Games;

namespace Trusted.Core.Entities;

public class User : IdentityUser<long>
{
    public string IpAddress { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public string ProfilePic { get; set; }
    public double Rating { get; set; } = 0;
    public int AccountsSold { get; set; }=0;
    public int AccountsBought { get; set; } = 0;
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<AccountMediator> MediatedAccounts { get; set; } = new List<AccountMediator>();
    public ICollection<Deal> Deals { get; set; } = new List<Deal>();
    public ICollection<Deal> MediatedDeals { get; set; } = new List<Deal>();
}
