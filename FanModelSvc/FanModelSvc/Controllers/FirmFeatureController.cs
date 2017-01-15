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
    [MyAuthorization]
    public class FirmFeatureController : ApiController
    {
        private FeatureFlagRepository flagRepo;
        private TokenRepository tokenRepo;
        public FirmFeatureController()
        {
            flagRepo = new FeatureFlagRepository();
            tokenRepo = new TokenRepository();
        }

        public bool Get(string flag)
        {
            return Get(flag, false);
        }
        public bool Get(string flag, bool defaultvalue)
        {
            try
            {
                var cookies = Request.Headers.GetCookies("Security");
                foreach (var cookie in cookies)
                {
                    var token = cookie.Cookies.FirstOrDefault(x => x.Name.ToLower() == "security");
                    if (token != null)
                    {
                        var id = new Guid(token.Value);
                        
                        var userName = tokenRepo.GetUserName(id);

                        if (!string.IsNullOrEmpty(userName))
                        {
                            return flagRepo.CheckFlag(userName, flag, defaultvalue);
                        }

                    }
                }

                return false;
            }
            catch            {
                //TODO: Log / warn about whatever error is occuring.
                return defaultvalue;
            }
        }
    }
}