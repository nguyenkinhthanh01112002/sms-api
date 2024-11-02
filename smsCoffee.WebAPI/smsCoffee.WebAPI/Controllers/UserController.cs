using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.DTOs.Common;
using smsCoffee.WebAPI.DTOs.Common.Panigations;
using smsCoffee.WebAPI.DTOs.UserDto;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Models;
using smsCoffee.WebAPI.Services.CommonService;

namespace smsCoffee.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpPost("CreateUser")]

        public async Task<ActionResult<ApiResponse<UserResponseDto>>> CreatUser([FromBody] AddingUserRequest request)
        {
            try
            {
                var userModel = await _userService.CreateUserAsync(request);
                if (userModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFactory.Error<UserResponseDto>(Request.Path, "Email existed", StatusCodes.Status400BadRequest));
                }
                var data = _mapper.Map<UserResponseDto>(userModel);
                return StatusCode(StatusCodes.Status201Created, ResponseFactory.Success(Request.Path, data, "A new user created successfully", StatusCodes.Status201Created));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating user");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<UserResponseDto>(Request.Path, "An error occurred while creating the user", StatusCodes.Status500InternalServerError));
            }
        }
        //     [Authorize]
        /*[HttpGet("GetUsers")]
        public async Task<ActionResult<ApiResponse<List<UserResponseDto>>>> GetUsers()
        {
            try
            {
                var appUsers = await _userService.GetUsersAsync();
                var userResponseDto = _mapper.Map<List<UserResponseDto>>(appUsers);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, userResponseDto, "Get users success", StatusCodes.Status200OK));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get users failed");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<List<UserResponseDto>>(Request.Path, "Get users fail", StatusCodes.Status500InternalServerError));
            }
        }*/
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> UpdateUserByIdAsync(string id, UpdatingRequestDto request)
        {
            try
            {
                var user = await _userService.UpdateUserByIdAsync(id, request);
                var data = _mapper.Map<UserResponseDto>(user);
                if(user != null)
                {
                    return StatusCode(StatusCodes.Status200OK,ResponseFactory.Success(Request.Path, data,$"Get user by {id} success"));
                }
                return StatusCode(StatusCodes.Status400BadRequest, ResponseFactory.Error<UserResponseDto>(Request.Path, $"Get user by {id} fail"));
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An exceptional occured");
                return StatusCode(StatusCodes.Status500InternalServerError,e.Message);
            }
        }               
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CustomPagedResponse<UserResponseDto>>>> GetUsers([FromQuery] PanigationParameters parameters)
        {
            try
            {
                if (parameters.PageNumber <= 0 || parameters.PageSize <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFactory.Error<CustomPagedResponse<UserResponseDto>>(Request.Path, "pageSize or pageNumber invalid", StatusCodes.Status400BadRequest));
                }
                var results = await _userService.GetUsersAsync(parameters);
                var data = _mapper.Map<CustomPagedResponse<UserResponseDto>>(results);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data, "Get Users succcess", StatusCodes.Status200OK));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get users failed");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<CustomPagedResponse<UserResponseDto>>(Request.Path, "Get Users failed", StatusCodes.Status500InternalServerError));
            }

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> DeleteUserById(string id)
        {
            try
            {
                var user = await _userService.DeleteUserByIdAsync(id);
                if(user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseFactory.Error<UserResponseDto>(Request.Path, $"{id} was not found", StatusCodes.Status404NotFound));
                }

                var data = _mapper.Map<UserResponseDto>(user);
                return StatusCode(StatusCodes.Status200OK,ResponseFactory.Success(Request.Path, data,$"Delete user {id} success",StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An exception happened");
               return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<CustomPagedResponse<UserResponseDto>>(Request.Path, "Get Users failed", StatusCodes.Status500InternalServerError));
            }
        }
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> DeleteBulkUsersById([FromBody]DeletingBulkUserRequest request)
        {
            try
            {
                var users = await _userService.DeleteBulkUsersByIdAsync(request);
                if(users == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseFactory.Error<UserResponseDto>(Request.Path,"Delete users fail",StatusCodes.Status404NotFound));
                }
                var data = _mapper.Map<List<UserResponseDto>>(users);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception happend");
                return StatusCode(StatusCodes.Status500InternalServerError,ResponseFactory.Error<UserResponseDto>(Request.Path,"Delete bulk users fail",StatusCodes.Status500InternalServerError));
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if( user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,ResponseFactory.Error<UserResponseDto>(Request.Path,$"Get user by {id} fail",StatusCodes.Status404NotFound));
                }
                var data = _mapper.Map<UserResponseDto>(user) ;
                return StatusCode(StatusCodes.Status200OK,ResponseFactory.Success(Request.Path,$"Get user by {id} fail"));
            }
            catch(Exception e)
            {
                _logger.LogError(e,"An exception happened");
                return StatusCode(StatusCodes.Status500InternalServerError,ResponseFactory.Error<UserResponseDto>(Request.Path,"Get user by id fail"));
            }
        }
    }
}
