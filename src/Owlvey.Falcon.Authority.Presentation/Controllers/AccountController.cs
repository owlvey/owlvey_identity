using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Authority.Presentation.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("singlesignout")]
        public IActionResult singlesignout(string returnUrl)
        {
            var siteCookies = HttpContext.Request.Cookies.Where(c => c.Key.Contains("AspNetCore") || c.Key.Contains("Identity.Application.session"));
            foreach (var cookie in siteCookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }

            return Redirect(returnUrl);
        }
    }
}
