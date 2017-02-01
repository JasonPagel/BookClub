using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FanModelSvc.Models;
using FanModelSvc.Services;

namespace FanModelSvc.Controllers
{
    public class UserFeatureController : ApiController
    {
        private UserFeatureRepository _repo;

        public UserFeatureController()
        {
            _repo = new UserFeatureRepository();
        }

        [HttpGet]
        public bool Get(string flag, string user)
        {
            return Get(flag, user, false);
        }
        [HttpGet]
        public bool Get(string flag, string user, bool defaultvalue)
        {
            try
            {
                return _repo.CheckFlag(flag, user, defaultvalue);
            }
            catch
            {
                //TODO: Log / warn about whatever error is occuring.
                return defaultvalue;
            }
        }



        [HttpPost]
        public HttpResponseMessage Post(Flag flag)
        {
            if (string.IsNullOrEmpty(flag.name))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (!_repo.SetFlag(flag))
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }



    }
}