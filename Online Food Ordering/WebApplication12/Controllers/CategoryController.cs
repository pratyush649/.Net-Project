using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class CategoryController : Controller
    {
        WebDBEntities _db = new WebDBEntities();
        // GET: Category
        public ActionResult ManageCategory()
        {

            return View();
        }

        public JsonResult GetData()
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<CategoryViewModel> lst = new List<CategoryViewModel>();
                var catList = db.tbl_Category.ToList();
                foreach (var item in catList)
                {
                    lst.Add(new CategoryViewModel() { categoryID = item.categoryID, categoryName = item.categoryName });
                }
                return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (WebDBEntities db = new WebDBEntities())
                {
                    ViewBag.Action = "New Category";
                    return View(new CategoryViewModel());
                }
            }
            else
            {
                using (WebDBEntities db = new WebDBEntities())
                {
                    CategoryViewModel sub = new CategoryViewModel();
                    var menu = db.tbl_Category.Where(x => x.categoryID == id).FirstOrDefault();
                    sub.categoryID = menu.categoryID;
                    sub.categoryName = menu.categoryName;
                    ViewBag.Action = "Edit Category";
                    return View(sub);
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(CategoryViewModel sm)
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                if (sm.categoryID == 0)
                {
                    tbl_Category tb = new tbl_Category();
                    tb.categoryName = sm.categoryName;
                    db.tbl_Category.Add(tb);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tbl_Category tbm = db.tbl_Category.Where(m => m.categoryID == sm.categoryID).FirstOrDefault();
                    tbm.categoryName = sm.categoryName;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                tbl_Category sm = db.tbl_Category.Where(x => x.categoryID == id).FirstOrDefault();
                db.tbl_Category.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}