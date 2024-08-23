using AutoMapper;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Mappers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        { 
            CreateMap<Role,RoleDto>().ReverseMap();
            CreateMap<Role,AddingRoleDto>().ReverseMap();
            CreateMap<Role,UpdatingRoleDto>().ReverseMap();
        }
    }
}
