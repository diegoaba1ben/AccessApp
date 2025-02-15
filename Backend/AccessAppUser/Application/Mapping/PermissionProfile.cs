using AutoMapper;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Permission;

namespace AccessAppUser.Application.Mapping
{
    public class PermissionProfile : AutoMapper.Profile 
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionReadDTO>().ReverseMap();
            CreateMap<PermissionCreateDTO, Permission>();
        }
    }
}
