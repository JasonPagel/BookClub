using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanModelSvc.Models;

namespace FanModelSvc.Services
{
    public class UserRepository
    {
        private const string CacheKey = "UserStore";

        public UserRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var users = new UserLogin[] {
                        new UserLogin() {name="jason", password="test" },
                        new UserLogin() {name="joe", password="test" }
                    };

                    ctx.Cache[CacheKey] = users;
                }
            }
        }

        public bool AddUser(UserLogin user)
        {

            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((UserLogin[])ctx.Cache[CacheKey]).ToList();
                    var existingUser = currentData.FirstOrDefault(x => x.name == user.name);
                    if (existingUser != null)
                    {
                        Console.WriteLine("User already exists");
                        return false;
                    }

                    currentData.Add(user);

                    ctx.Cache[CacheKey] = currentData.ToArray();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }

            return false;
        }

        public UserLogin GetUser(string name, string password)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((UserLogin[])ctx.Cache[CacheKey]).ToList();
                    var foundUser = currentData.FirstOrDefault(x => x.name.ToLower() == name.ToLower() && x.password == password);
                    return foundUser;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }

            return null;
        }
    }
}