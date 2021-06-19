using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.BlazorServer.Controllers
{
    [Route("[controller]/[action]")]
    public class CultureController : Controller
    {
        public IActionResult SetCulture(string culture, string redirectUri)
        {
            if(culture is not null)
            {
                HttpContext.Response.Cookies
                    .Append(CookieRequestCultureProvider.DefaultCookieName, 
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));
                //store a cookie that the user want this site in en-USA
            }

            return LocalRedirect(redirectUri);//you have to redirect to the same uri ur in now

        }
    }
}
