using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int RoleId);
        Task<Role?> UpdateRoleByIdAsync(int RoleId, UpdatingRoleDto updatingRoleDto);
        Task<Role> CreatingRoleAsync(AddingRoleDto addingRoleDto);
        Task<Role?> DeletingRoleByIdAsync(int RoleId);
    }
}
