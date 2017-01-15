using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FanModelSvc.Models
{
    public class Book
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public Guid authorid { get; set; }
        public string authorname { get; set; }
        public string description { get; set; }
        public int pages { get; set; }
        public DateTime published { get; set; }
        public string image { get; set; }
    }
}