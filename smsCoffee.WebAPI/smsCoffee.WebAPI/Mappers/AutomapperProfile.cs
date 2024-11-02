using AutoMapper;
using Microsoft.AspNetCore.Identity;
using smsCoffee.WebAPI.DTOs.CategoryDtos;
using smsCoffee.WebAPI.DTOs.Common.Panigations;
using smsCoffee.WebAPI.DTOs.ProductDtos;
using smsCoffee.WebAPI.DTOs.RoleDto;
using smsCoffee.WebAPI.DTOs.UserDto;
using smsCoffee.WebAPI.Models;

namespace smsCoffee.WebAPI.Mappers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        { 
            CreateMap<IdentityRole,RoleDto>().ForMember(dest => dest.roleId, opt => opt.MapFrom(src => src.Id)).
                ForMember(dest => dest.roleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<AppUser, UserResponseDto>().ReverseMap();
            // Map PagedResponse<AppUser> sang PagedResponse<UserResponseDto>
            CreateMap<PagedResponse<AppUser>, CustomPagedResponse<UserResponseDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Data));
            CreateMap<Category, CategoryDto>();
            CreateMap<Product, ProductDto>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))  ;
        }
    }
}
