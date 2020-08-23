using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class OrderDetailViewModel
    {
        public int orderDetailID { get; set; }
        public Nullable<int> orderID { get; set; }
        public Nullable<int> productID { get; set; }
        public string title { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<decimal> unitPrice { get; set; }
    }
}