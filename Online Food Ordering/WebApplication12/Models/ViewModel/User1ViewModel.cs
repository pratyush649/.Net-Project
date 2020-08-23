using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class User1ViewModel
    {
        public int userID { get; set; }

        [Required]
        public string fullname { get; set; }
        [Required]
        public string gender { get; set; }
        [Required]
        public string email { get; set; }
    }
}