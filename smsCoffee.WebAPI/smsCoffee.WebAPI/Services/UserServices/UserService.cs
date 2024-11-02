using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.DTOs.Common.Panigations;
using smsCoffee.WebAPI.DTOs.UserDto;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Services
{ 
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AppUser?> CreateUserAsync(AddingUserRequest request)
        {
            var appUser = new AppUser
            {
                UserName = request.email,
                Email = request.email,
                FullName = request.fullName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            //tao tai khoan mac dinh dung dinh dang
            var result = await _userManager.CreateAsync(appUser, "Thanh123@");
            if (result.Succeeded)
            {               
                return appUser;
            }
            return null;
        }
        public async Task<AppUser?> DeleteUserAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return user;
                }
            }
            return null;
        }
       public async Task<AppUser?> UpdateUserByIdAsync(string id,UpdatingRequestDto request)
       {
            var user = await _userManager.FindByIdAsync(id);
              if (user != null)
            {
                user.FullName = !string.IsNullOrEmpty(request.fullName) ? request.fullName : user.FullName;
                user.Address = !string.IsNullOrEmpty(request.address) ? request.address : user.Address;
               if(request.dateOfBirth != null)
               {
                    user.DateOfBirth = request.dateOfBirth;
                    user.Age = (int)(DateTime.Now.Year - user.DateOfBirth.Value.Year);
               }
                if (request.isActive ?? false)
                {
                    user.IsActive = 1;
                }
                else
                {
                    user.IsActive = 0;
                }
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return user;
                }
            }
            return null;
       }
        public async Task<AppUser?> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<AppUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task<PagedResponse<AppUser?>> GetUsersAsync(PanigationParameters parameters)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {

                query = query.Where(u =>
                    u.FullName.Contains(parameters.SearchTerm) || u.Address.Contains(parameters.SearchTerm) || u.Age.ToString().Contains(parameters.SearchTerm));
            }
            if (parameters.SortOrderAsc??true)
            {
                query = query.OrderBy(u => u.UserName ?? string.Empty);           
            }
            else
            {
                query = query.OrderByDescending(u => u.UserName ?? string.Empty);
            }

            // Đếm tổng số bản ghi
            var totalRecords = await query.CountAsync();

            // Áp dụng phân trang
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResponse<AppUser?>(
                items,
                parameters.PageNumber,
                parameters.PageSize,
                totalRecords);
        }
        public async Task<AppUser?> DeleteUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return user;
                }               
            }
            return null;
        }
        public async Task<List<AppUser>?> DeleteBulkUsersByIdAsync(DeletingBulkUserRequest request)
        {
            if(request.Ids != null && request.Ids.Count>0)
            {
                var users = new List<AppUser>();
                foreach (var id in request.Ids)
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        await _userManager.DeleteAsync(user);
                        users.Add(user);
                    }
                }
                return users;
            }
            else
            {
               return null;
            }          
        }
    }
}
