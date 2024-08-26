using smsCoffee.WebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.UserDto
{
    public class UserDto
    {
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }   
        public string FullName { get; set; }     
        public string ContactInfo { get; set; }     
        public int RoleId { get; set; }
       // public Role Role { get; set; }
       
        public byte IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
