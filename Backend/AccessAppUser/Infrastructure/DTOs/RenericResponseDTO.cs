using System;

namespace AccessAppUser.Infrastructure.DTOs
{
    /// <summary>
    /// DTO Genérico para estandarizar respuestas en la API.
    /// </summary>
    /// <typeparam name="T">Tipo de dato que será envuelto en la respuesta.</typeparam>
    public class GenericResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } // Código de estado HTTP
        public T? Data { get; set; }

        public GenericResponseDTO(bool success, int statusCode, string message, T? data = default)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Genera una respuesta de éxito con datos.
        /// </summary>
        public static GenericResponseDTO<T> SuccessResponse(T data, int statusCode = 200, string message = "Operación exitosa") =>
            new GenericResponseDTO<T>(true, statusCode, message, data);

        /// <summary>
        /// Genera una respuesta de éxito sin datos.
        /// </summary>
        public static GenericResponseDTO<T> SuccessResponse(int statusCode = 200, string message = "Operación exitosa") =>
            new GenericResponseDTO<T>(true, statusCode, message);

        /// <summary>
        /// Genera una respuesta de error.
        /// </summary>
        public static GenericResponseDTO<T> ErrorResponse(int statusCode, string message) =>
            new GenericResponseDTO<T>(false, statusCode, message);
    }
}
