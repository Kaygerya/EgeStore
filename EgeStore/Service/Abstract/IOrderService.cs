using EgeStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Service.Abstract
{
    public interface IOrderService
    {
        Order GetOrderById(string Id);

        List<Order> GetOrdersByUserId(string userId);

        void Insert(Order order);
    }
}
