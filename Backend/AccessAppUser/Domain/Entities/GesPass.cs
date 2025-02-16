using System;
using System.ComponentModel.DataAnnotations;
using AccessAppUser.Infrastructure.Helpers;

namespace AccessAppUser.Domain.Entities
{
    /// <summary>
    /// Entidad para gestionar cambios de contraseñas de usuarios.
    /// </summary>
    public class GesPass
    {
        [Key] // Clave primaria
        public Guid Id { get; private set; } // Se manejará en el Builder
        public Guid UserId { get; set; } // Id del usuario asociado
        public User User { get; private set; } = null!; // Relación de navegación hacia User

        public string ResetToken { get;  set; } = string.Empty; // Token para resetear la contraseña
        public DateTime TokenExpiration { get; set; } // Fecha de expiración del token
        public bool IsCompleted { get; private set; } // Indica si el cambio de contraseña fue completado
        public DateTime? CompletedAt { get; set; } // Fecha de completado para auditoría

        /// <summary>
        /// Constructor privado para evitar instanciación directa.
        /// </summary>
        public GesPass() { }

        /// <summary>
        /// Método estático para inicializar el Builder.
        /// </summary>
        public static GesPassBuilder Builder() => new GesPassBuilder();

        /// <summary>
        /// Builder interno para construir instancias de GesPass.
        /// </summary>
        public class GesPassBuilder
        {
            private readonly GesPass _gesPass;

            public GesPassBuilder()
            {
                _gesPass = new GesPass
                {
                    Id = GuidProvider.NewGuid(), // Se usa GuidProvider para consistencia
                    IsCompleted = false // Inicialmente, no está completado
                };
            }

            public GesPassBuilder WithUser(User user)
            {
                _gesPass.User = user ?? throw new ArgumentNullException(nameof(user));
                _gesPass.UserId = user.Id; // Asociar el ID del usuario
                return this;
            }

            public GesPassBuilder WithResetToken(string token, DateTime expiration)
            {
                _gesPass.ResetToken = token.Trim();
                _gesPass.TokenExpiration = expiration;
                return this;
            }

            public GesPassBuilder MarkAsCompleted()
            {
                _gesPass.IsCompleted = true;
                _gesPass.CompletedAt = DateTime.UtcNow;
                return this;
            }

            public GesPass Build() => _gesPass;
        }
    }
}