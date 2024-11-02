using Microsoft.AspNetCore.Identity;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Interfaces
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetRolesAsync();
        Task<IdentityRole?> GetRoleByIdAsync(string roleId);
        Task<IdentityRole?> UpdateRoleByIdAsync(string roleId, UpdatingRoleDto updatingRoleDto);
        Task<IdentityRole?> CreateRoleAsync(AddingRoleDto addingRoleDto);
        Task<IdentityRole?> DeleteRoleByIdAsync(string roleId);
    }
}
