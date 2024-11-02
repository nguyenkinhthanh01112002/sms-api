using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.RoleDto
{
    public class AddingRoleDto
    {
        [Required]
        public string roleName { get; set; }
    }
}
