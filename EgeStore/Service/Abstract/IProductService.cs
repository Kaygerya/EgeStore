using EgeStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Service.Abstract
{
    public interface IProductService
    {
        List<Product> GetAllProducts();

        void Insert(Product product);

        Product GetProductById(string productId);

        List<Product> GetProductsById(List<string> selectedIds);
    }
}
