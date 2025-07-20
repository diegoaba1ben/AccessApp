using System;

namespace AccessAppUser.Infrastructure.Exceptions
{
    /// <summary>
    /// Clase base para excepciones controladas en la aplicación.
    /// Proporciona trazabilidad mediante un identificador único.
    /// </summary>
    public abstract class AppException : Exception
    {
        /// <summary>
        /// Identificador único de la excepción para trazabilidad.
        /// </summary>
        public Guid TraceId { get; } = Guid.NewGuid();

        /// <summary>
        /// Constructor básico con mensaje personalizado.
        /// </summary>
        /// <param name="message">Mensaje de error explicativo.</param>
        protected AppException(string message)
            : base(message) { }

        /// <summary>
        /// Constructor que incluye una excepción interna (encadenamiento).
        /// </summary>
        /// <param name="message">Mensaje de error explicativo.</param>
        /// <param name="innerException">Excepción original que causó el error.</param>
        protected AppException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}