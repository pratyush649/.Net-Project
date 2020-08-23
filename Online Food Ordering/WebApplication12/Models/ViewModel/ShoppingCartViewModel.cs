using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<tbl_Cart> cartItem { get; set; }
        public decimal cartTotal { get; set; }
    }
}