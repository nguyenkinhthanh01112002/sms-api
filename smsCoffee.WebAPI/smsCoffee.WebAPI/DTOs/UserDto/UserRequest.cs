using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.UserDto
{
    public class UserRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string  fullName{ get; set; }
    }
}
