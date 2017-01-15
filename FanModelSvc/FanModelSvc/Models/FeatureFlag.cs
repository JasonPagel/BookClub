using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FanModelSvc.Models
{
    public class FeatureFlag
    {
        public string username { get; set; }
        public string flag { get; set; }
        public bool value { get; set; }
    }
}