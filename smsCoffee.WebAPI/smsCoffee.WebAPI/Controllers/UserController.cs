using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.DTOs.UserDto;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Services;

namespace smsCoffee.WebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(_mapper.Map<List<UserDto>>(users));
        }
        [HttpGet]
        [Route("{UserId:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] int UserId)
        {
            var user = await _userService.GetUserByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreatingNewUser([FromBody] AddingUserDto addingUserDto)
        {
            var user = await _userService.CreatingUserAsync(addingUserDto);
            var userDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetUserById), new { UserId = userDto.UserId }, userDto);
        }
        [HttpPut]
        [Route("{UserId:int}")]
        public async Task<IActionResult> DeletingUserById([FromRoute] int UserId, [FromBody] UpdatingUserDto updatingUserDto)
        {
            var user = await _userService.UpdateUserByIdAsync(UserId, updatingUserDto);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
        [HttpDelete]
        [Route("{UserId:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int UserId)
        {
            var user = await _userService.DeletingUserByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
