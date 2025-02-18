using System;
using System.Collections.Generic;

namespace AccessAppUser.Infrastructure.DTOs.Common
{
    public class GenResponseDTO<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public GenResponseDTO(T data, int statusCode, string message)
        {
            Data = data;
            StatusCode = statusCode;
            Message = message;
        }
        public static GenResponseDTO<T> SuccesResponse(T data, int statusCode, string message)
        {
           return new GenResponseDTO<T>(data, statusCode, message); 
        }
        public static GenResponseDTO<T> ErrorResponse(int statusCode, string message)
        {
            return new GenResponseDTO<T>(default!, statusCode,message);
        }
    }

}