using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FanModelSvc.Services;

namespace FanModelSvc.Services
{

    public class MyAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }

        private bool AuthorizeRequest(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            var values = headers.GetValues("Cookie");

            var repo = new TokenRepository();

            foreach (var value in values)
            {
                var cookies = value.Split(";".ToCharArray());
                foreach (var cookie in cookies)
                {
                    if (cookie.ToLower().StartsWith("security="))
                    {
                        var token = cookie.Split("=".ToCharArray());
                        var id = new Guid(token[1]);

                        if (repo.ValidateUser(id))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}