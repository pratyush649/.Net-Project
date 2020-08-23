using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models
{
    public class MyMenu
    {
        public static List<tbl_Category> GetMenus()
        {
            using (var context = new WebDBEntities())
            {
                return context.tbl_Category.ToList();
            }
        }
        public static List<tbl_Category> GetSubMenus(int menuid)
        {
            using (var context = new WebDBEntities())
            {
                return context.tbl_Category.Where(u => u.categoryID == menuid).ToList();
            }
        }
    }
}