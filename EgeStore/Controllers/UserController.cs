using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Armut.Iyzipay;
using Armut.Iyzipay.Model;
using Armut.Iyzipay.Request;
using EgeStore.Data.Models;
using EgeStore.Models.Users;
using EgeStore.Service.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace EgeStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IProductService _productService;

        private readonly IOrderService _orderService;

        public UserController(IUserService userService, IProductService productService, IOrderService orderService)
        {
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var result = _userService.LoginUser(model);

            if (result.IsSuccess)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Id),
                        new Claim(ClaimTypes.Role, model.IsAdmin.ToString() )
                    };
                ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                model.Error = result.Error;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterModel model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {

            var result =  _userService.RegisterUser(model);
            if (result.IsSuccess)
            {
                LoginModel loginModel = new LoginModel();
                loginModel.Username = model.Username;
                loginModel.Password = model.Password;
                return Login(loginModel);
            }
            else
            {
                return View(model);
            }

        }

        [HttpGet]
        public  IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public IActionResult AddProductToCart(string id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(k => k.Type == ClaimTypes.Name).Value;
            var user = _userService.GetUserById(userId);

            if(user.Cart.Where(k=> k.Id == id).FirstOrDefault() == null)
            {
                user.Cart.Add(new Data.Models.ShoppingCartItem { Id = id, Quantity = 1 });
            }
            else
            {
                user.Cart.Where(k => k.Id == id).FirstOrDefault().Quantity++;
            }

            _userService.Update(user);
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult Cart(PaymentModel paymentModel = null)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(k => k.Type == ClaimTypes.Name).Value;
            var user = _userService.GetUserById(userId);
            List<ShoppingCartItemModel> model = new List<ShoppingCartItemModel>();
            if (user.Cart.Count > 0)
            {
              var products = _productService.GetProductsById(user.Cart.Select(k => k.Id).ToList());
                foreach(var sci in user.Cart)
                {
                    ShoppingCartItemModel item = new ShoppingCartItemModel();
                    var product = products.Where(k => k.Id == sci.Id).FirstOrDefault();
                    if(product != null)
                    {
                        item.Name = product.Name;
                        item.Quantity = sci.Quantity;
                        item.ImageUrl = product.GetPath();
                        item.Price = product.Price;
                        model.Add(item);
                    }
                }
            }
            if (model == null)
            {
                paymentModel = new PaymentModel();
            }
            paymentModel.Items = model;
            return View(paymentModel);
        }

        [HttpPost]
        //https://github.com/EmreKarahan/Iyzipay.Core
        //https://sandbox-merchant.iyzipay.com/transactions
        public IActionResult Pay(PaymentModel model)
        {
            decimal total = decimal.Zero;
            var userId = HttpContext.User.Claims.FirstOrDefault(k => k.Type == ClaimTypes.Name).Value;
            var user = _userService.GetUserById(userId);
            if (user.Cart.Count > 0)
            {
                var products = _productService.GetProductsById(user.Cart.Select(k => k.Id).ToList());
                foreach (var sci in user.Cart)
                {
                    var product = products.Where(k => k.Id == sci.Id).FirstOrDefault();
                    if (product != null)
                    {
                        total += (product.Price * sci.Quantity);
                    }
                }
            }

            Options options = new Options();
            options.ApiKey = "sandbox-H80OCnhufjSVxqD9N10iIcOoYvqnIm33";
            options.SecretKey = "sandbox-MoUs6Qzjs7UALRtAvcBf2KpL0sTi9jMG";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = total.ToString();
            request.PaidPrice = total.ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = user.Username;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.LastUsageMonth;
            paymentCard.ExpireYear = model.LastUsageYear;
            paymentCard.Cvc = model.Cvv;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;


            List<BasketItem> basketItems = new List<BasketItem>();

            if (user.Cart.Count > 0)
            {
                var products = _productService.GetProductsById(user.Cart.Select(k => k.Id).ToList());
                foreach (var sci in user.Cart)
                {
                    var product = products.Where(k => k.Id == sci.Id).FirstOrDefault();
                    if (product != null)
                    {
                        BasketItem firstBasketItem = new BasketItem();
                        firstBasketItem.Id = product.Id;
                        firstBasketItem.Name = product.Name;
                        firstBasketItem.Category1 = "Collectibles";
                        firstBasketItem.Category2 = "Accessories";
                        firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                        firstBasketItem.Price = (product.Price * sci.Quantity).ToString();
                        basketItems.Add(firstBasketItem);
                    }

                }
            }
            request.BasketItems = basketItems;

            Payment payment = Payment.Create(request, options);
            if(payment.Status == Status.SUCCESS.ToString())
            {
                model.SuccessMessage = "Alışverişiniz başarılı";

                //sepeti boşalt
                user.Cart = new List<Data.Models.ShoppingCartItem>();
                _userService.Update(user);

                Order order = new Order();
                order.OrderTotal = total;
                order.Address = shippingAddress;
                order.BasketItems = basketItems;
                order.UserId = user.Id;
                _orderService.Insert(order);

                return View(model);
            }
            else
            {
                model.Error = payment.ErrorMessage;
                return RedirectToAction("Cart", model);
            }

        }

        public IActionResult MyOrders()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(k => k.Type == ClaimTypes.Name).Value;
            var user = _userService.GetUserById(userId);
            List<Order> orders = _orderService.GetOrdersByUserId(user.Id);
            return View(orders);


        }
        
    }
}