using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.AccountDto
{
    public class ResetPasswordDto
    {
        /*    public string Email { get; set; }
            public string Code { get; set; }
            public string NewPassword { get; set; }*/
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
