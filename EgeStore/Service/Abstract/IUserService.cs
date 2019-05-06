using EgeStore.Data.Base;
using EgeStore.Data.Models;
using EgeStore.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeStore.Service.Abstract
{
    public interface IUserService
    {
        User GetUserById(string Id);

        User GetUserByUsername(string username);

        void Insert(User user);

        Entity RegisterUser(RegisterModel model);

        Entity LoginUser(LoginModel model);
    }
}
