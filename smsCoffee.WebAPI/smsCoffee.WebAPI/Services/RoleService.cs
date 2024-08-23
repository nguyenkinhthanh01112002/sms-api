using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.Data;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly CoffeeDbContext _context;
        private readonly IMapper _mapper;
        public RoleService(CoffeeDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Role> CreatingRoleAsync(AddingRoleDto addingRoleDto)
        {
           var role = _mapper.Map<Role>(addingRoleDto);
           await _context.Roles.AddAsync(role);
           _context.SaveChanges();
           return role;
        }

        public async Task<Role?> DeletingRoleByIdAsync(int RoleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == RoleId);
            if (role == null) {return null;}
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
           return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int RoleId)
        {
           return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == RoleId);
        }

        public async Task<Role?> UpdateRoleByIdAsync(int RoleId, UpdatingRoleDto updatingRoleDto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == RoleId);
            if(role == null) {return null;}
            role.RoleName = updatingRoleDto.RoleName;
            await _context.SaveChangesAsync();
            return role;
        }
    }
}
