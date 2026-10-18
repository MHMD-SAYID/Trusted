using Microsoft.AspNetCore.Identity;
using Trusted.Core.Entities.Games;

namespace Trusted.Core.Entities;

public class User : IdentityUser<long>
{
    public string IpAddress { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public ICollection<Account> Accounts { get; set; }=new List<Account>();
}
