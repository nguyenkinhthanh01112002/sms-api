using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string ContactInfo { get; set; }
        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        [Required]
        public byte IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
