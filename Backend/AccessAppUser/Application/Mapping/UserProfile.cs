using AutoMapper;
using AccessAppUser.Domain.Entities;
using AccessAppUser.Infrastructure.DTOs.User;

namespace AccessAppUser.Application.Mapping
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDTO>().ReverseMap();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
        }
    }
}
