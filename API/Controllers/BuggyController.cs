using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }
        
        [HttpGet("notfound")]

        public ActionResult GetNotFound()
        {
            var thing = _context.Products.Find(0);

            if(thing is null)
                return NotFound(new ApiResponses(404));

            return Ok();
        }


        [HttpGet("servererror")]

        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(0);

           var anotherThing = thing.ToString();

            return Ok();
        }

        [HttpGet("badrequest")]

        public ActionResult GetBadRequest()
        {
            var thing = _context.Products.Find(0);

            if (thing is null)
                return BadRequest(new ApiResponses(400));

            return Ok();
        }


        [HttpGet("badrequest/{id}")]

        public ActionResult GetNotFound(int id)
        {
            var thing = _context.Products.Find(0);

            if (thing is null)
                return NotFound();

            return Ok();
        }
    }
}
