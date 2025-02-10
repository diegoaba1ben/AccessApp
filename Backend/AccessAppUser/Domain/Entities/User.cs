using System;
using System.Collections.Generic;
using AccessAppUser.Infrastructure.Helpers;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Representa un usuario dentro del sistema.
    /// </summary>
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public static bool IsSystemInitialized { get; private set; } = false;

        // Relaciones
        public Profile Profile { get; private set; } = null!;
        public List<Role> Roles { get; private set; } = new();
        public GesPass GesPass { get; private set; } = null!; 

        /// <summary>
        /// Constructor privado para evitar instanciación directa.
        /// </summary>
        private User() { }

        /// <summary>
        /// Método estático para inicializar el Builder.
        /// </summary>
        public static UserBuilder Builder() => new UserBuilder();

        /// <summary>
        /// Builder interno para construir instancias de User.
        /// </summary>
        public class UserBuilder
        {
            private readonly User _user;

            public UserBuilder()
            {
                _user = new User { Id = GuidProvider.NewGuid() };
            }

            public UserBuilder WithName(string name)
            {
                _user.Name = name.Trim();
                return this;
            }

            public UserBuilder WithEmail(string email)
            {
                _user.Email = email.Trim().ToLower();
                return this;
            }

            public UserBuilder WithPassword(string password)
            {
                _user.Password = password;
                return this;
            }

            public UserBuilder CreatedAt(DateTime createdAt)
            {
                _user.CreatedAt = createdAt;
                return this;
            }

            public UserBuilder WithProfile(Profile profile)
            {
                _user.Profile = profile;
                return this;
            }

            public UserBuilder WithRoles(IEnumerable<Role> roles)
            {
                if (!User.IsSystemInitialized && (roles == null || !roles.Any()))
                {
                    // Si es el primer usuario, se le asigna el rol de Administrador
                    roles = new List<Role> {Role.Builder().SetName("Admin").SetDescription("Rol inicial del sistema.").Build()};
                }
                else if (roles == null || !roles.Any())
                {
                    throw new ArgumentException("El usuario debe tener al menos un rol.");
                }
                _user.Roles = new List<Role>(roles);
                return this;
            }

            public UserBuilder WithGesPass(GesPass gesPass)
            {
                _user.GesPass = gesPass;
                return this;
            }

            public User Build()
            {
                if (!User.IsSystemInitialized)
                {
                    User.IsSystemInitialized = true; // Se marca el sistema como inicializado  
                }
                return _user;
            } 
        }
    }
}