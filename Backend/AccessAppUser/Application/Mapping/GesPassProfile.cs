using AutoMapper;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.GesPass;

namespace AccessAppUser.Application.Mapping

{
    public class GesPassProfile : AutoMapper.Profile // 🔥 Evita conflicto con 'Profile' en 'Entities'
    {
        public GesPassProfile()
        {
            CreateMap<GesPass, GesPassReadDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ReverseMap();

            CreateMap<GesPassCreateDTO, GesPass>();
        }
    }
}
