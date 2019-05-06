using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EgeStore.Models;

namespace EgeStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                return RedirectToAction("Index","Product");
            }
        } 
    }
}
