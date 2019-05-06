using EgeStore.Data;
using EgeStore.Data.Base;
using EgeStore.Data.Models;
using EgeStore.Models.Users;
using EgeStore.Service.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Service
{
    public class UserService : IUserService
    {
        public User GetUserById(string Id)
        {
            MongoContext context = new MongoContext();
            var user = context.Users.Find(x => x.Id == Id).FirstOrDefault();
            return user;
        }

        public User GetUserByUsername(string username)
        {
            MongoContext context = new MongoContext();
            var user = context.Users.Find(x => x.Username == username).FirstOrDefault();
            return user;
        }

        public void Insert(User user)
        {
            MongoContext context = new MongoContext();
            context.Users.InsertOne(user);
        }

        public Entity RegisterUser(RegisterModel model)
        {

            if (string.IsNullOrEmpty(model.Username))
            {
                model.Error = "Username cannot be empty!";
                return model;
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                model.Error = "Password can not be empty!";
                return model;
            }

            if (model.Password.Length < 8 )
            {
                model.Error = "Password must be at least 8 characters";
                return model;
            }

            if (model.Password != model.PasswordAgain)
            {
                model.Error = "Password must be at least 8 characters";
                return model;
            }

            var user = GetUserByUsername(model.Username.ToLower());
            if (user != null)
            {
                model.Error = "User already exists!";
                return model;
            }

            user = new User();
            user.Username = model.Username;
            user.Password = model.Password;
            Insert(user);

            return new Entity();
        }

        public Entity LoginUser(LoginModel model)
        {
            var user = GetUserByUsername(model.Username.ToLower());
            if(user == null)
            {
                model.Error = "User not found!";
                return model;
            }
            if(user.Password != model.Password)
            {
                model.Error = "Wrong password!";
                return model;
            }
            model.Id = user.Id;
            return model;
        }
    }
}
