using EgeStore.Data;
using EgeStore.Data.Models;
using EgeStore.Service.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Service
{
    public class OrderService : IOrderService
    {
        public Order GetOrderById(string Id)
        {
            MongoContext context = new MongoContext();
            var order = context.Orders.Find(x => x.Id == Id).FirstOrDefault();
            return order;
        }

        public List<Order> GetOrdersByUserId(string userId)
        {
            MongoContext context = new MongoContext();
            var orders = context.Orders.Find(x => x.UserId == userId).ToList();
            return orders;
        }

        public void Insert(Order order)
        {
            MongoContext context = new MongoContext();
            context.Orders.InsertOne(order);
        }
    }
}
