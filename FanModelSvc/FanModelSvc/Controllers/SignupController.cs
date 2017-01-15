using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FanModelSvc.Models;
using FanModelSvc.Services;

namespace FanModelSvc.Controllers
{
    public class SignupController : ApiController
    {
        private UserRepository repo;

        public SignupController()
        {
            repo = new UserRepository();
        }

        [HttpPost]
        public HttpResponseMessage Post(UserLogin user)
        {
            repo.AddUser(user);
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
