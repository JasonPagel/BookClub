using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanModelSvc.Models;

namespace FanModelSvc.Services
{
    public class TokenRepository
    {
        private const string CacheKey = "TokenStore";

        public TokenRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var users = new UserToken[] { };
                    ctx.Cache[CacheKey] = users;
                }
            }
        }

        public bool RemoveUser(Guid id)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((UserToken[])ctx.Cache[CacheKey]).ToList();
                    var user = FindUser(id, currentData);
                    if (user != null)
                    {
                        currentData.Remove(user);
                        ctx.Cache[CacheKey] = currentData.ToArray();
                    }

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

        public string GetUserName(Guid id)
        {
            var user = FindUser(id);

            if (user != null)
                return user.name;

            return string.Empty;
        }
        public bool ValidateUser(Guid id)
        {
            var user = FindUser(id);

            if (user != null)
                return true;
            
            return false;
        }

        private UserToken FindUser(Guid id)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((UserToken[])ctx.Cache[CacheKey]).ToList();
                    return FindUser(id, currentData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return null;
        }
        private UserToken FindUser(Guid id, IEnumerable<UserToken> users)
        {
            var found = users.FirstOrDefault(x => x.id == id);

            return found;
        }

        public bool SaveUser(UserToken user)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {

                    var currentData = ((UserToken[])ctx.Cache[CacheKey]).ToList();
                    var existingUser = currentData.FirstOrDefault(x => x.name == user.name);
                    if (existingUser != null)
                    {
                        currentData.Remove(existingUser);
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
    }
}