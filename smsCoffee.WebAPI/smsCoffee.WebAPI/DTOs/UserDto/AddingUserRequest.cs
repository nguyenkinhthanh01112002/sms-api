using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.UserDto
{
    public class AddingUserRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string email { get; set; }
    }
}
