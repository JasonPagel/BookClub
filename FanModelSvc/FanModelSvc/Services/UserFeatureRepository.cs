using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FanModelSvc.Models;

namespace FanModelSvc.Services
{
    public class UserFeatureRepository
    {

        private const string CacheKey = "UserFlagStore";

        public UserFeatureRepository()
        {
            var currentData = GetCurrentData();
            if (currentData == null)
            {
                var flags = new Flag[]
                {
                    new Flag() {name="search", value=true },
                    new Flag() {name="delete", value=true }
                };

                CacheData(flags);
            }
        }

        public bool CheckFlag(string flag)
        {
            var currentData = GetCurrentData();
            var foundFlag = currentData.FirstOrDefault(x => x.name.ToLower() == flag.ToLower());
            if (foundFlag == null)
                return false;

            return foundFlag.value;
        }

        public bool SetFlag(Flag flag)
        {
            var currentData = GetCurrentData();
            var foundFlag = currentData.FirstOrDefault(x => x.name.ToLower() == flag.name.ToLower());

            if (foundFlag != null)
                foundFlag.value = flag.value;
            else
                currentData.Add(flag);

            CacheData(currentData);

            return true;
        }

        private List<Flag> GetCurrentData()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                try
                {
                    var currentData = ((Flag[])ctx.Cache[CacheKey]).ToList();
                    return currentData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            return null;
        }
        private bool CacheData(IEnumerable<Flag> flags)
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                ctx.Cache[CacheKey] = flags.ToArray();
                return true;
            }

            return false;
        }
    }
}