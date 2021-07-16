using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Khalifa.Framework
{
    public interface IApiResponse
    {
        int StatusCode { get; set; }

        string? ReasonMessage { get; set; }

        Exception? Exception { get; set; }
    }

    public abstract class ApiResponse : IApiResponse
    {
        public bool IsOK => StatusCode == (int) HttpStatusCode.OK;

        public bool IsError => StatusCode == (int)HttpStatusCode.InternalServerError;

        public bool IsNotFound => StatusCode == (int)HttpStatusCode.NotFound;

        public int StatusCode { get; set; }

        public string? ReasonMessage { get; set; }

        public Exception? Exception { get; set; }

        public static T Error<T>(int statusCode, string reasonMessage, Exception ex) where T : ApiResponse, new()
        {
            return new T
            {
                StatusCode = statusCode,
                ReasonMessage = reasonMessage,
                Exception = ex
            };
        }

        public static T Error<T>(string reasonMessage, Exception ex) where T : ApiResponse, new()
        {
#if DEBUG
            return Error<T>((int)HttpStatusCode.InternalServerError, ex?.Message ?? "", ex);
#else
            return Error<T>(StatusCodes.Status500InternalServerError, reasonMessage, ex);
#endif
        }
    }
}
