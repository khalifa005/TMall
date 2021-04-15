using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.MediatoR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {

        [HttpGet]
        public string GetProducrs()
        {
            return "list of products";
        }
        
        [HttpGet("{id}")]
        public string GetProducr(int id)
        {
            return "one of products";
        }
    }
}
