using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.AccountDto
{
    public class RegisterDto
    {
        
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string ?PhoneNumber { get; set; }
        [Required]
        public string PassWord { get; set; }
        
    }
}
