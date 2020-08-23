using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class CategoryViewModel
    {
        public int categoryID { get; set; }

        [Required]
        public string categoryName { get; set; }
    }
} 