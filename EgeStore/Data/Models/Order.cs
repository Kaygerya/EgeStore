using Armut.Iyzipay.Model;
using EgeStore.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgeStore.Data.Models
{
    public class Order :Entity
    {
        public Order()
        {
            Address = new Address();
            BasketItems = new List<BasketItem>();
        }
        public string UserId { get; set; }

        public Address Address { get; set; }

        public  List<BasketItem> BasketItems { get; set; }

        public decimal OrderTotal { get; set; }

        public DateTime CreatedDate { get; set; }

        public string AddressString()
        {
            StringBuilder addressline = new StringBuilder();
            addressline.AppendLine(this.Address.ContactName);
            addressline.AppendLine(this.Address.Description);
            addressline.AppendLine(this.Address.ZipCode);
            addressline.AppendLine(this.Address.City + "/" + this.Address.Country);
            return addressline.ToString();
        }
    }
}
