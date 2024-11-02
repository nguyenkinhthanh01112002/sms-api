using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.AccountDto
{
    public class LoginDto
    {

        [Required]
        public string EmailOrPhone { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
