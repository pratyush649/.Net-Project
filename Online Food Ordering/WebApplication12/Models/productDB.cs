using WebApplication12.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models
{
    public static class productDB
    {
        public static List<tbl_Product> GetAllRecentItem()
        {
            using (var context = new WebDBEntities())
            {
                return context.tbl_Product.Where(s => s.isSpecial == "Special").Take(4).ToList();
            }
        }
        public static List<tbl_Product> GetAllFoodRecentItem()
        {
            using (var context = new WebDBEntities())
            {
                return context.tbl_Product.Take(8).ToList();
            }
        }

    }
}