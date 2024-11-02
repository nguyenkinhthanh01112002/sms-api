using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.DTOs.AccountDto;
using smsCoffee.WebAPI.DTOs.UserDto;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Models;
using smsCoffee.WebAPI.Utilities;

namespace smsCoffee.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        public AccountController(UserManager<AppUser> userManager,ITokenService tokenService,SignInManager<AppUser> signInManager,IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if(string.IsNullOrEmpty(registerDto.Email)&&string.IsNullOrEmpty(registerDto.PhoneNumber))
                    return BadRequest("Either Email or Phone Number must be provided.");

                var appUser = new AppUser
                {
                    UserName = registerDto.Email ?? registerDto.PhoneNumber,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.PhoneNumber
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.PassWord);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        var accessToken = await _tokenService.CreateToken(appUser);
                        var refreshToken = _tokenService.CreateRefreshToken();

                        appUser.RefreshToken = refreshToken;
                        appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Set expiry to 7 days from now

                        await _userManager.UpdateAsync(appUser);

                        return Ok(new
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            PhoneNumber = appUser.PhoneNumber,
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        });
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
              
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await FindUserByEmailOrPhoneAsync(loginDto.EmailOrPhone);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var userDto = await GenerateUserDtoAsync(user);
            var refreshToken = _tokenService.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);
            // Assuming token expiration is set to 1 hour (3600 seconds)
            int expiresIn = 3600;



            return Ok(new LoginResponseDto
            {
                AccessToken = userDto.Token,
                RefreshToken = refreshToken,
                ExpireTime = expiresIn
            });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.EmailOrPhone);
            if (user == null)
                return Ok(); // Don't reveal that the user does not exist

            var verificationCode = VerificationCodeGenerator.GenerateCode();
            user.PasswordResetToken = verificationCode;
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(1); // Token expires after 1 hour

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return StatusCode(500, "An error occurred while processing your request.");

            await _emailService.SendEmailAsync(user.Email, "Password Reset Verification Code",
                $"Your verification code is: {verificationCode}. This code will expire in 1 hour.");

            return Ok(new { verificationCode });
        }

        [HttpPost("verify-reset-otp")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.EmailOrPhone  );
            if (user == null || user.PasswordResetToken != dto.Otp ||
                user.PasswordResetTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired code.");
            }

            return Ok(new { emailOrPhone = dto.EmailOrPhone, code = dto.Otp });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto, [FromQuery] string otp)
        {
            if (string.IsNullOrEmpty(otp))
            {
                return BadRequest(new {otp = "InvalidToken", description = "Mã xác thực không được để trống." });
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == otp);
            if (user == null)
            {
                return BadRequest(new { otp = "InvalidToken", description = "Mã xác thực không hợp lệ hoặc đã hết hạn." });
            }

            if (user.PasswordResetTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest(new { otp = "InvalidToken", description = "Mã xác thực đã hết hạn." });
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Tiếp tục xử lý reset password nếu token hợp lệ
            var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);
            if (result.Succeeded)
            {
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiration = null;
                await _userManager.UpdateAsync(user);
                return Ok(new { message = "Đặt lại mật khẩu thành công." });
            }

            return BadRequest(result.Errors);
        }
        private async Task<AppUser> FindUserByEmailOrPhoneAsync(string emailOrPhone)
        {
            return emailOrPhone.Contains("@")
                ? await _userManager.FindByEmailAsync(emailOrPhone)
                : await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == emailOrPhone);
        }

        private async Task<UserDto> GenerateUserDtoAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Token = await _tokenService.CreateToken(user),
                Roles = roles.ToList()
            };
        }
        
    }
}
