using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class LoginViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool rememberme { get; set; }
    }
}