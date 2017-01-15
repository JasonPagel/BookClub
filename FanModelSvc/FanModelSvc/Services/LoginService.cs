using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanModelSvc.Models;

namespace FanModelSvc.Services
{
    public class LoginService
    {
        private UserRepository userRepo;
        private TokenRepository tokenRepo;

        public LoginService()
        {
            userRepo = new UserRepository();
            tokenRepo = new TokenRepository();
        }

        public UserToken ValidateUser(UserLogin user)
        {
            var foundUser = userRepo.GetUser(user.name, user.password);
            if (foundUser == null)
                return null;

            var token = new UserToken(user);
            if (tokenRepo.SaveUser(token))
                return token;

            return null;
        }

    }
}