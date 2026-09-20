using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Trusted.ViewModel.DTO.Request.Auth;

public class RegisterRequest
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
    public string IpAddress { get; set; }
    public string PhoneNumber { get; set; }
}
