using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter07.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
        public string ReturnUrl { get; set; }
    }
}
