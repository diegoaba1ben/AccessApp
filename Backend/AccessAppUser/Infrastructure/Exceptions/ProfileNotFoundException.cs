using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra un perfil con el ID especificado.
    /// </summary>
    public class ProfileNotFoundException : AppException
    {
        public Guid ProfileId { get; }

        public ProfileNotFoundException(Guid profileId)
            : base($"No se encontró ningún perfil con el ID '{profileId}'.")
        {
            ProfileId = profileId;
        }
    }
}