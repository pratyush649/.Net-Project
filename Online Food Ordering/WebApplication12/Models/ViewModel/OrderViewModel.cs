using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WebApplication12.Models.ViewModel
{
    public class OrderViewModel
    {
        public int orderID { get; set; }
        public string username { get; set; }
        [Required(ErrorMessage = "Firstname Required")]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Lastbname Required")]
        public string lastname { get; set; }
        [Required(ErrorMessage = "Address Required")]
        public string address { get; set; }
        [Required(ErrorMessage = "Phone number Required")]
        public string phone { get; set; }
        public string total { get; set; }
        public string orderDate { get; set; }
        public string delivayStatus { get; set; }
    }
}