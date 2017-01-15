using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanModelSvc.Models;

namespace FanModelSvc.Services
{
    public class FeatureFlagRepository
    {

        private const string CacheKey = "FlagStore";

        public FeatureFlagRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if(ctx.Cache[CacheKey] == null)
                {
                    var flags = new FeatureFlag[]
                    {
                        new FeatureFlag() {username="jason", flag="delete", value=true },
                        new FeatureFlag() {username="jason", flag="book", value=true },
                        new FeatureFlag() {username="jason", flag="search", value=true },
                        new FeatureFlag() {username="joe", flag="search", value=true }
                    };
                    ctx.Cache[CacheKey] = flags;
                }
            }
        }

        public bool CheckFlag(string username, string flag, bool defaultValue)
        {

            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var flags = ((FeatureFlag[])ctx.Cache[CacheKey]).ToList();
                    var foundFlag = flags.FirstOrDefault(x => x.username == username && x.flag == flag);
                    if (foundFlag != null)
                        return foundFlag.value;
                }
                catch (Exception ex)
                {
                    // on error return the default flag
                    Console.WriteLine(ex.ToString());
                    return defaultValue;
                }
            }

            // if no flag was found for that user return false;
            return false;
        }
    }
}