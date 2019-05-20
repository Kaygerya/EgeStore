using EgeStore.Data.Base;
using EgeStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Models.Users
{
    public class ShoppingCartItemModel : ShoppingCartItem
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class PaymentModel : Entity
    {
        public PaymentModel()
        {
            Items = new List<ShoppingCartItemModel>();
        }
        public List<ShoppingCartItemModel> Items { get; set; }

        public string CardNumber { get; set; }
        public string LastUsageMonth { get; set; }
        public string LastUsageYear { get; set; }
        public string Cvv { get; set; }

        public string SuccessMessage { get; set; }
    }
}
