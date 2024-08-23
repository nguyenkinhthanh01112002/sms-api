using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.Interfaces;

namespace smsCoffee.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(_mapper.Map<List<RoleDto>>(roles));
        }
        [HttpGet]
        [Route("{RoleId:int}")]
        public async Task<IActionResult> GetRoleById([FromRoute] int RoleId)
        {
            var role = await _roleService.GetRoleByIdAsync(RoleId);
            if (role == null)
            {
                return NotFound();
            }
            var roleDto = _mapper.Map<RoleDto>(role);
            return Ok(roleDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreatingNewRole([FromBody] AddingRoleDto addingRoleDto)
        {
            var role = await _roleService.CreatingRoleAsync(addingRoleDto);
            var roleDto = _mapper.Map<RoleDto>(role);
            return CreatedAtAction(nameof(GetRoleById), new { RoleId = roleDto.RoleId }, roleDto);
        }
        [HttpPut]
        [Route("{RoleId:int}")]
        public async Task<IActionResult> DeletingRoleById([FromRoute]int RoleId,[FromBody]UpdatingRoleDto updatingRoleDto)
        {
            var role = await _roleService.UpdateRoleByIdAsync(RoleId, updatingRoleDto);
            if(role == null)
            {
                return NotFound();
            }
            var roleDto = _mapper.Map<RoleDto>(role);
            return Ok(roleDto);
        }
        [HttpDelete]
        [Route("{RoleId:int}")]
        public async Task<IActionResult> DeleteRole([FromRoute]int RoleId)
        {
            var role = await _roleService.DeletingRoleByIdAsync(RoleId);
            if(role == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        
    }
}
