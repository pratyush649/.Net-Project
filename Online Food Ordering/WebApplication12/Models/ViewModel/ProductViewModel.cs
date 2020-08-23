using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class ProductViewModel
    {
        public int productID { get; set; }
        [Required(ErrorMessage = "title Required")]
        public string title { get; set; }
        [Required(ErrorMessage = "Category Required")]
        public string categoryName { get; set; }

        [Required(ErrorMessage = "description Required")]
        public string description { get; set; }

        [Required(ErrorMessage = "sellingPrice Required")]
        public Nullable<decimal> sellingPrice { get; set; }

        [Required(ErrorMessage = "unitPrice Required")]
        public Nullable<decimal> unitPrice { get; set; }

        public Nullable<int> categoryID { get; set; }
        public string photo { get; set; }

        public string isSpecial { get; set; }
    }
}