using AccessAppUser.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;



namespace AccessAppUser.Application.Mapping
{
    /// <summary>
    /// Configuración global de AutoMapper para registrar todos los perfiles
    /// </summary>
    public static class MapperConfig
    {
        public static IServiceCollection AddApplicationMappers(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserProfile());
                    mc.AddProfile(new RoleProfile());
                    mc.AddProfile(new AreaProfile());
                    mc.AddProfile(new PermissionProfile());
                    mc.AddProfile(new GesPassProfile());
                });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;       
        }
    }
}