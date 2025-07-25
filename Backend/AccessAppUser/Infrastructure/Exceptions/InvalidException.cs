using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se proporciona un identificador inválido para una entidad.
    /// </summary>
    public class InvalidIdException : AppException
    {
        public string ParameterName { get; }
        public Type EntityType { get; }

        public InvalidIdException(string parameterName, Type entityType)
            : base($"El parámetro '{parameterName}' no es válido para la entidad '{entityType.Name}'.")
        {
            ParameterName = parameterName;
            EntityType = entityType;
        }
    }
}