using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EgeStore.Models;
using EgeStore.Service.Abstract;
using Microsoft.AspNetCore.Hosting;
using EgeStore.Areas.Admin.Models;

namespace EgeStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IProductService productService, IHostingEnvironment hostingEnvironment)
        {
            _productService = productService;
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            var products = _productService.GetAllProducts().Select(k => new ProductModel { Id = k.Id, Name = k.Name, Price = k.Price }).ToList();

            return View(products);
        } 
    }
}
