using AutoMapper;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Role;

namespace AccessAppUser.Application.Mapping
{
    public class RoleProfile : AutoMapper.Profile 
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleReadDTO>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => rp.Permission.Name)))
                .ReverseMap();

            CreateMap<RoleCreateDTO, Role>();
        }
    }
}
