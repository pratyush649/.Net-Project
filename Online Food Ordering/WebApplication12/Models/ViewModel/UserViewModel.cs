using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class UserViewModel
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
   
        public string fullname { get; set; }
        public string email { get; set; }
    }
}