using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FanModelSvc.Models
{
    public class UserToken
    {
        public UserToken(string name)
        {
            this.id = Guid.NewGuid();
            this.name = name;
            this.expiration = DateTime.Now.AddMinutes(30);
        }
        public UserToken(UserLogin user)
        {
            this.id = Guid.NewGuid();
            this.name = user.name;
            this.expiration = DateTime.Now.AddMinutes(30);
        }
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime expiration { get; set; }
    }
}