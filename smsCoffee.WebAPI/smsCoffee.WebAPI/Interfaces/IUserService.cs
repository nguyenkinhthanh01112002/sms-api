using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.DTOs.UserDto;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int UserId);
        Task<User?> UpdateUserByIdAsync(int UserId, UpdatingUserDto updatingUserDto);
        Task<User> CreatingUserAsync(AddingUserDto addingUserDto);
        Task<User?> DeletingUserByIdAsync(int UserId);
    }
}
