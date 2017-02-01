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
                    new Flag() {name="search", value=true, user="jason" },
                    new Flag() {name="delete", value=true, user="jason" },
                    new Flag() {name="search", value=true, user="bill" },
                    new Flag() {name="delete", value=true, user="bill" }
                };

                CacheData(flags);
            }
        }

        public bool CheckFlag(string flag, string user, bool defaultValue)
        {
            var foundFlag = GetFlag(flag, user);
            if (foundFlag == null)
                return defaultValue;

            return foundFlag.value;
        }

        public bool SetFlag(Flag flag)
        {
            var currentData = GetCurrentData();
            var foundFlag = GetFlag(flag.name, flag.user, currentData);

            if (foundFlag != null)
                foundFlag.value = flag.value;
            else
                currentData.Add(flag);

            CacheData(currentData);

            return true;
        }

        private Flag GetFlag(string flag, string user)
        {
            var currentData = GetCurrentData();
            return GetFlag(flag, user, currentData);
        }
        private Flag GetFlag(string flag, string user, IEnumerable<Flag> flags)
        {
            var foundFlag = flags.FirstOrDefault(x => x.name.ToLower() == flag.ToLower() && x.user.ToLower() == user.ToLower());
            return foundFlag;
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