using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FanModelSvc.Models;
using FanModelSvc.Services;
using System.Net.Http.Headers;

namespace FanModelSvc.Controllers
{
    public class LoginController : ApiController
    {
        private TokenRepository repo;

        public LoginController()
        {
            repo = new Services.TokenRepository();
        }

        [HttpPost]
        public HttpResponseMessage Post(UserLogin user)
        {
            var loginSvc = new LoginService();
            var token = loginSvc.ValidateUser(user);

            if (token == null)
                throw new HttpResponseException(HttpStatusCode.Unauthorized);


            if (!repo.SaveUser(token))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            var response = Request.CreateResponse<UserToken>(System.Net.HttpStatusCode.Accepted, token);

            var secCookie = new CookieHeaderValue("Security", token.id.ToString());
            secCookie.HttpOnly = true;
            secCookie.Expires = token.expiration;

            response.Headers.AddCookies(new CookieHeaderValue[] { secCookie });

            return response;
        }
        
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            if (!repo.RemoveUser(id))
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            var response = Request.CreateResponse(HttpStatusCode.OK);

            return response;
        }
        
    }
}
