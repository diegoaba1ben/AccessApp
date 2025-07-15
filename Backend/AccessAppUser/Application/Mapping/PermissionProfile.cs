using AutoMapper;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Permission;

namespace AccessAppUser.Application.Mapping
{
    public class PermissionProfile : AutoMapper.Profile

    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionReadDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => rp.Role.Name)))
                .ReverseMap();

            CreateMap<PermissionCreateDTO, Permission>();
        }
    }
}
