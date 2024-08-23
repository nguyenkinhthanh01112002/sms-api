using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.Models
{
    public class Role
    {
        public int RoleId { get; set; }  
        
        [Required]
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
