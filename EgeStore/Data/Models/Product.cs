using EgeStore.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Data.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string GetPath()
        {
            return "/images/Products/" + this.Id + ".jpg";

        }
    }
}
