using System.ComponentModel.DataAnnotations;

namespace smsCoffee.WebAPI.DTOs.RoleDto
{
    public class UpdatingRoleDto
    {
        [Required]
        public string roleName { get; set; }
    }
}
