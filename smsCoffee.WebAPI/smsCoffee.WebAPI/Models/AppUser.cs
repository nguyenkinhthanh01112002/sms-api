using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smsCoffee.WebAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiration { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth {  get; set; }
        public string? Address { get; set; }
        public int ? Age {  get; set; }
        public byte? IsActive { get; set; }
        public DateTime? CreatedAt {  get; set; }
        public DateTime? UpdatedAt {  get; set; }
    }
}
