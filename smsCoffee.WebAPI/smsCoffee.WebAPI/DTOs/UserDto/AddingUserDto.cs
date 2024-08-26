using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.UserDto
{
    public class AddingUserDto
    {
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string ContactInfo { get; set; }
        [Required]
        public int RoleId { get; set; }
        public byte IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

    }
}
