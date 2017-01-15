using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FanModelSvc.Models
{
    public class Author
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int bookcount { get; set; }
        public string image { get; set; }
    }
}