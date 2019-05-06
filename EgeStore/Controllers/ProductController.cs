using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EgeStore.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(k => k.Type == ClaimTypes.Name).Value;
            return View();
        }
    }
}