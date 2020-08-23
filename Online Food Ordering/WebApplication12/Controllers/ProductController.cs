using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        WebDBEntities _db = new WebDBEntities();
        public ActionResult ManageProduct()
        {
            return View();
        }
        public JsonResult GetData()
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<ProductViewModel> lstitem = new List<ProductViewModel>();
                var lst = db.tbl_Product.Include("tbl_Category").ToList();
                foreach (var item in lst)
                {
                    lstitem.Add(new ProductViewModel() { productID = item.productID, categoryName = item.tbl_Category.categoryName, title = item.title, unitPrice = item.unitPrice, sellingPrice = item.sellingPrice, photo = item.photo, description=item.description });
                }
                return Json(new { data = lstitem }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (WebDBEntities db = new WebDBEntities())
                {
                    ViewBag.Categories = db.tbl_Category.ToList();

                    return View(new ProductViewModel());
                }
            }
            else
            {
                using (WebDBEntities db = new WebDBEntities())
                {

                    ViewBag.Categories = db.tbl_Category.ToList();
                    tbl_Product item = db.tbl_Product.Where(i => i.productID == id).FirstOrDefault();
                    ProductViewModel itemvm = new ProductViewModel();
                    itemvm.productID = item.productID;
                    itemvm.categoryID = Convert.ToInt32(item.categoryID);
                    itemvm.title = item.title;
                    itemvm.unitPrice = item.unitPrice;
                    itemvm.sellingPrice = item.sellingPrice;
                    itemvm.description = item.description;
                    itemvm.photo = item.photo;

                    return View(itemvm);
                }
            }
        }

        [HttpPost]

        public ActionResult AddOrEdit(ProductViewModel ivm)
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                if (ivm.productID == 0)
                {
                    tbl_Product itm = new tbl_Product();

                    itm.categoryID = Convert.ToInt32(ivm.categoryID);
                    itm.title = ivm.title;
                    itm.unitPrice = ivm.unitPrice;
                    itm.sellingPrice = ivm.sellingPrice;
                    itm.description = ivm.description;
                    HttpPostedFileBase fup = Request.Files["photo"];
                    if (fup != null)
                    {
                        if (fup.FileName != "")
                        {
                            fup.SaveAs(Server.MapPath("~/ProductImages/" + fup.FileName));
                            itm.photo = fup.FileName;
                        }
                    }

                    itm.isSpecial = ivm.isSpecial;
                    db.tbl_Product.Add(itm);
                    db.SaveChanges();
                    ViewBag.Message = "Created Successfully";
                }
                else
                {
                    tbl_Product itm = db.tbl_Product.Where(i => i.productID == ivm.productID).FirstOrDefault();
                    itm.categoryID = Convert.ToInt32(ivm.categoryID);
                    itm.title = ivm.title;
                    itm.unitPrice = ivm.unitPrice;
                    itm.sellingPrice = ivm.sellingPrice;
                    itm.description = ivm.description;
                    HttpPostedFileBase fup = Request.Files["Photo"];
                    if (fup != null)
                    {
                        if (fup.FileName != "")
                        {
                            fup.SaveAs(Server.MapPath("~/ProductImages/" + fup.FileName));
                            itm.photo = fup.FileName;
                        }
                    }



                    db.SaveChanges();
                    ViewBag.Message = "Updated Successfully";

                }
                ViewBag.Categories = db.tbl_Category.ToList();
                return View(new ProductViewModel());

            }


        }

        [HttpPost]

        public ActionResult Delete(int id)
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                tbl_Product sm = db.tbl_Product.Where(x => x.productID == id).FirstOrDefault();
                db.tbl_Product.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}