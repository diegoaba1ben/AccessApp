using AutoMapper;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.Area;

namespace AccessAppUser.Application.Mapping
{
    public class AreaProfile : AutoMapper.Profile // 🔥 Evita conflicto con 'Profile' en 'Entities'
    {
        public AreaProfile()
        {
            CreateMap<Area, AreaReadDTO>()
                .ForMember(dest => dest.AssociatedRoles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)))
                .ReverseMap();

            CreateMap<AreaCreateDTO, Area>();
        }
    }
}
