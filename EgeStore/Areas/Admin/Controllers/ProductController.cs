using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EgeStore.Areas.Admin.Models;
using EgeStore.Data.Models;
using EgeStore.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EgeStore.Areas.Admin.Controllers
{

    //https://dev.iyzipay.com/tr
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IProductService productService, IHostingEnvironment hostingEnvironment)
        {
            _productService = productService;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        public IActionResult Index()
        {
            var isAdmin = Convert.ToBoolean( HttpContext.User.Claims.FirstOrDefault(k => k.Type == ClaimTypes.Role).Value);
            if (!isAdmin)
                RedirectToAction("NotAuthorize");

            var productmodels = _productService.GetAllProducts().Select(k => new ProductModel { Id = k.Id, Name = k.Name, Price = k.Price }).ToList();

            return View(productmodels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductModel model = new ProductModel();
            return View(model);
        }


        [HttpPost]
        public IActionResult Create(ProductModel model, IFormFile Picture)
        {

            Product product = new Product();
            product.Name = model.Name;
            product.Price = model.Price;
            _productService.Insert(product);

            var filePath = _hostingEnvironment.WebRootPath + "\\images\\Products\\" + product.Id + ".jpg";
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                Picture.CopyToAsync(fileStream);
            }

                return View(model);
        }


        public IActionResult NotAuthorize()
        {
            return View();
        } 
    }
}