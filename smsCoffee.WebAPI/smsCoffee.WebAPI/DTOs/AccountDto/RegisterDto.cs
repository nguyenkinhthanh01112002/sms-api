using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.AccountDto
{
    public class RegisterDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PassWord { get; set; }
        
    }
}
