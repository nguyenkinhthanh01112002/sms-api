using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.DTOs.Common.Panigations;
using smsCoffee.WebAPI.DTOs.UserDto;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Interfaces
{
    public interface IUserService
    {
        public Task<AppUser?> CreateUserAsync(AddingUserRequest request);
        public Task<AppUser?> GetUserByIdAsync(string id);
        public Task<PagedResponse<AppUser?>> GetUsersAsync(PanigationParameters parameters);
        public Task<AppUser?> DeleteUserByIdAsync (string id);
        public Task<List<AppUser>?> DeleteBulkUsersByIdAsync([FromBody] DeletingBulkUserRequest request);
        public Task<AppUser?> UpdateUserByIdAsync(string id,UpdatingRequestDto request);
    }
}
