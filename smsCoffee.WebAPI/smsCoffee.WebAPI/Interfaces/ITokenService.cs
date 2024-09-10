using smsCoffee.WebAPI.Models;
using System.Security.Claims;

namespace smsCoffee.WebAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        string CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
