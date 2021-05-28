using API.Errors;
using Khalifa.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("errors/{code}")]
    public class ErrorController : BaseApiController
    {
        //use the middleweare to redirect to this end point
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponses(code));
        }
    }
}
