using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class ShoppingCartRemoveViewModel
    {
        public string message { get; set; }
        public decimal cartTotal { get; set; }
        public int cartCount { get; set; }
        public int itemCount { get; set; }
        public int deleteID { get; set; }
    }
}