using AutoMapper;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.Data;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.DTOs.UserDto;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly CoffeeDbContext _context;
        public UserService(IMapper mapper, CoffeeDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<User> CreatingUserAsync(AddingUserDto addingUserDto)
        {
            var user = _mapper.Map<User>(addingUserDto);
            await _context.Users.AddAsync(user);
            _context.SaveChanges();
            return user;
        }

        public async Task<User?> DeletingUserByIdAsync(int UserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
            if (user == null) { return null; }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
           return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int UserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
        }

        public async Task<User?> UpdateUserByIdAsync(int UserId, UpdatingUserDto updatingUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
            if (user == null) { return null; }
            user.FullName = updatingUserDto.FullName;
            user.Email = updatingUserDto.Email;
            user.ContactInfo = updatingUserDto.ContactInfo;
            user.CreatedTime = updatingUserDto.CreatedTime;
            user.RoleId = updatingUserDto.RoleId;
            user.UpdatedTime = updatingUserDto.UpdatedTime; 
            user.IsActive = updatingUserDto.IsActive;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
