using Microsoft.AspNetCore.Identity;

namespace smsCoffee.WebAPI.DTOs.UserDto
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public byte? IsActive {  get; set; }
        public List<string?> Roles { get; set; }
    }
}
