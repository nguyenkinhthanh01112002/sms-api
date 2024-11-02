using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.DTOs.Common;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.Interfaces;
using smsCoffee.WebAPI.Services.CommonService;

namespace smsCoffee.WebAPI.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;
        private readonly ILogger<RoleController> _logger;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService, ILogger<RoleController> logger,IMapper mapper)
        {
            this.roleService = roleService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRole(AddingRoleDto addingRoleDto)
        {
            try
            {
                var role = await roleService.CreateRoleAsync(addingRoleDto);
                if (role == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ResponseFactory.Error<RoleDto>(Request.Path,"Invalid Input",StatusCodes.Status400BadRequest));
                }
                var data = _mapper.Map<RoleDto>(role);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data, "Create a new role", StatusCodes.Status200OK));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An excepiton occured in creating role");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<RoleDto>(Request.Path, "An excepiton occured in creating role",StatusCodes.Status500InternalServerError));
            }
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<RoleDto>>>> GetRoles()
        {
            try
            {
                var roles = await roleService.GetRolesAsync();
                var data =  _mapper.Map<List<RoleDto>>(roles);
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path,data));
            }
            catch (Exception e)
            {
                _logger.LogError(e,"An exceptional occured while getting roles");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<RoleDto>(Request.Path, "Get roles fail", StatusCodes.Status500InternalServerError));
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleById(string id)
        {
            try
            {
                var role = await roleService.GetRoleByIdAsync(id);
                if(role == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,ResponseFactory.Error<RoleDto>(Request.Path,$"Get role by {id} fail",StatusCodes.Status404NotFound));
                }
                var data = _mapper.Map<RoleDto>(role);
                return StatusCode(StatusCodes.Status200OK,ResponseFactory.Success(Request.Path,data,$"Get role by {id} success",StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An exception occured while get role by {id}");
                return StatusCode(StatusCodes.Status500InternalServerError,ResponseFactory.Error<RoleDto>(Request.Path, $"An exception occured while get role by {id}",StatusCodes.Status500InternalServerError));
            }
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> UpdateRoleById(string id, UpdatingRoleDto updatingRoleDto)
        {
            try
            {
                var role = await roleService.UpdateRoleByIdAsync(id, updatingRoleDto);
                if(role == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseFactory.Error<RoleDto>(Request.Path, $"Updating role by Id: {id} fail", StatusCodes.Status404NotFound));
                }
                var data = _mapper.Map<RoleDto>(role) ;
                return StatusCode(StatusCodes.Status200OK, ResponseFactory.Success(Request.Path, data, $"Updateing role by id: {id} success", StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e,"An exceptional occured while updating role");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseFactory.Error<RoleDto>(Request.Path,$"Updaing by Id: {id} fail",StatusCodes.Status500InternalServerError));
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> DeleteRoleById(string id)
        {
            try
            {
                var role = await roleService.DeleteRoleByIdAsync(id);
                if( role == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound,ResponseFactory.Error<RoleDto>(Request.Path,$"Deleting role by {id} fail",StatusCodes.Status404NotFound));
                }
                var data = _mapper.Map<RoleDto>(role);
                return StatusCode(StatusCodes.Status200OK,ResponseFactory.Success(Request.Path,data,$"Deleting role by {id} success",StatusCodes.Status200OK));
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An exceptional occured while deleting role by id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError,ResponseFactory.Error<RoleDto>(Request.Path,$"Get role by id: {id} fail",StatusCodes.Status500InternalServerError));
            }
        }
    }
}
