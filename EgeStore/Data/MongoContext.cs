using EgeStore.Data.Models;
using EgeStore.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Data
{
    public class MongoContext
    {
        public MongoContext()
        {
            var connectionString = DbSettings.ConnectionString;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("egeuniv2");
        }
        private IMongoClient _client;
        private IMongoDatabase _database;

        public IMongoCollection<User> Users
        {
            get { return _database.GetCollection<User>("users"); }
        }

        public IMongoCollection<Product> Products
        {
            get { return _database.GetCollection<Product>("products"); }
        }

        public IMongoCollection<Order> Orders
        {
            get { return _database.GetCollection<Order>("orders"); }
        }
    }
}