using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.AccountDto
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string EmailOrPhone { get; set; }
    }
}
