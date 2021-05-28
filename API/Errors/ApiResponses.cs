using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiResponses
    {
        public ApiResponses(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }


        public int StatusCode { get; set; }
        public string Message { get; set; }


        private string GetDefaultMessageForStatusCode(int statusCode)
        {
           return statusCode switch
           {
               400 => "you have made a bad request",
               401 => "you are not authorized",
               404 => "resource not found",
               500 => "server error",
               _=> null
           };
        }
    }
}
