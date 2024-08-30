using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.DTOs.AccountDto;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.PassWord);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                               // Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
        [HttpGet("GetUserRoles/{userName}")]
        public async Task<IActionResult> GetUserRoles(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("Username cannot be null or empty.");
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound($"User '{userName}' not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles == null || !roles.Any())
            {
                return Ok(new { UserName = userName, Roles = new List<string>(), Message = "User has no roles assigned." });
            }

            return Ok(new { UserName = userName, Roles = roles });
        }

    }
}
