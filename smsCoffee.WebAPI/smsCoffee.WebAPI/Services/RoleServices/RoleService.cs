using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.Interfaces;

namespace smsCoffee.WebAPI.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }
        // Lấy tất cả roles
        public Task<List<IdentityRole>> GetRolesAsync()
        {
            return _roleManager.Roles.ToListAsync();
        }
        //create a new role
        public async Task<IdentityRole?> CreateRoleAsync(AddingRoleDto addingRoleDto)
        {
            var role = new IdentityRole
            {
                Name = addingRoleDto.roleName
            };
            
            var result = await _roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                return role;
            }
            return null;
        }
        //get role by id
        public async Task<IdentityRole?> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                return role;

            }
            return null;
        }
        public async Task<IdentityRole?> UpdateRoleByIdAsync(string roleId,UpdatingRoleDto updatingRoleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                role.Name = updatingRoleDto.roleName;

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return role;
                }
            }       
            return null;
        }
        public async Task<IdentityRole?> DeleteRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role != null)
            {
               var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return role;
                }
            }
            return null;
        }
    } 
}
