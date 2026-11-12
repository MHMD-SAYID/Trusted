using System.ComponentModel.DataAnnotations;

namespace Trusted.ViewModel.DTO.Request.User
{
    public class ChangePasswordReq
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

