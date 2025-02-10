using AccessAppUser.Domain.Entities;
using System;
using System.Collections.Generic;

namespace AccessAppUser.Application.Builders
{
    /// <summary>
    /// Clase que permite construir un usuario externo asegurando consistencia con las reglas del sistema.
    /// </summary>
    public class UserBuilderExternal
    {
        private readonly User.UserBuilder _builder;

        public UserBuilderExternal()
        {
            _builder = User.Builder();
        }

        public UserBuilderExternal WithName(string name)
        {
            _builder.WithName(name);
            return this;
        }

        public UserBuilderExternal WithEmail(string email)
        {
            _builder.WithEmail(email);
            return this;
        }

        public UserBuilderExternal WithPassword(string password)
        {
            _builder.WithPassword(password);
            return this;
        }

        public UserBuilderExternal WithProfile(Profile profile)
        {
            _builder.WithProfile(profile);
            return this;
        }

        public UserBuilderExternal WithRoles(List<Role> roles)
        {
            if (!User.IsSystemInitialized && (roles == null || roles.Count == 0))
            {
                // Si es el primer usuario del sistema, se le asigna el rol de Administrador
                roles = new List<Role>
                {
                    Role.Builder().SetName("Admin").SetDescription("Rol inicial del sistema").Build()
                };
            }
            else if (roles == null || roles.Count == 0)
            {
                throw new ArgumentException("El usuario debe tener al menos un rol asignado.");
            }

            _builder.WithRoles(roles);
            return this;
        }

        public User Build() => _builder.Build();
    }
}