﻿using System.Net;

namespace MagazineManagment.BLL.ResponseService
{
    public class ResponseService<T>
    {
        public string? ErrorMessage { get; set; }
        public T? Value { get; set; }
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }



        public ResponseService(string? errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ResponseService(string? errorMessage, T? value, bool success, HttpStatusCode statusCode)
        {
            ErrorMessage = errorMessage;
            Value = value;
            Success = success;
            StatusCode = statusCode;
        }

        public ResponseService(string? errorMessage, HttpStatusCode statusCode)
        {
            ErrorMessage = errorMessage;
            Value = default;
            Success = false;
            StatusCode = statusCode;
        }

        public static ResponseService<T> Ok(T value)
        {
            return new ResponseService<T>(String.Empty, value, true, HttpStatusCode.OK);
        }

        public static ResponseService<T> NotFound(string errorMessage)
        {
            return new ResponseService<T>(errorMessage, HttpStatusCode.NotFound);
        }

        public static ResponseService<T> Deleted(string Message)
        {
            return new ResponseService<T>(Message);
        }

        public static ResponseService<T> ExceptioThrow(string ThrowMessage)
        {
            return new ResponseService<T>(ThrowMessage);
        }
    }
}