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
        public bool Get(string flag)
        {
            return Get(flag, false);
        }
        public bool Get(string flag, bool defaultvalue)
        {
            try
            {
                //TODO: Business logic to call launch darkly and build up user/ firm objects based on auth.
                if (flag == "delete" || flag == "search")
                    return true;
                else
                    return false;
            }
            catch
            {
                //TODO: Log / warn about whatever error is occuring.
                return defaultvalue;
            }
        }
    }
}