using EgeStore.Data.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Data.Models
{
    [BsonIgnoreExtraElements]
    public class User : Entity
    {
        public User ()
        {
            Cart = new List<ShoppingCartItem>();
        }

        [DisplayName("Username")]
        public string Username { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public List<ShoppingCartItem> Cart { get; set; }

        public int CartItemCount { get {
                if (this.Cart == null)
                    return 0;

                return Cart.Sum(k => k.Quantity);
            } }
    }

    public class ShoppingCartItem
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}
