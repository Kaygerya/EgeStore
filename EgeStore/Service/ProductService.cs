using EgeStore.Data;
using EgeStore.Data.Models;
using EgeStore.Service.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Service
{
    public class ProductService : IProductService
    {

        public List<Product> GetAllProducts()
        {
            MongoContext context = new MongoContext();
            var products = context.Products.Find(new BsonDocument()).ToList();
            return products;
        }

        public Product GetProductById(string Id)
        {
            MongoContext context = new MongoContext();
            var product = context.Products.Find(x => x.Id == Id).FirstOrDefault();
            return product;
        }

        public void Insert(Product product)
        {
            MongoContext context = new MongoContext();
            context.Products.InsertOne(product);
        }

public List<Product> GetProductsById(List<string>selectedIds)
        {
            MongoContext dataContext = new MongoContext();
            var builder = Builders<Product>.Filter;
            FilterDefinition<Product> filter;
            filter = builder.Eq("_id", ObjectId.Parse(selectedIds[0]));
            for (int i = 1; i < selectedIds.Count(); i++)
            {
                filter = filter | builder.Eq("_id", ObjectId.Parse(selectedIds[i]));
            }
            var products = dataContext.Products.Find<Product>(filter);
            return products.ToList();
        }

    }
}
