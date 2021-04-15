﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class TestBaseApiController : BaseApiController
    {
        [HttpGet]
        public string Get()
        {
            return "worked";
        }
    }
}
