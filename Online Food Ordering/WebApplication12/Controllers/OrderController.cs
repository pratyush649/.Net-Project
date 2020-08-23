using WebApplication12.Models;

using WebApplication12.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace WebApplication12.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        // GET: Order

        public ActionResult ManageOrder()
        {
            return View();
        }
        public JsonResult GetData()
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<OrderViewModel> lstorder = new List<OrderViewModel>();
                var empList = db.tbl_Order.ToList();
                foreach (var item in empList)
                {
                    lstorder.Add(new OrderViewModel() { orderID = item.orderID, firstname = item.firstname, lastname = item.lastname, address = item.address, phone = item.phone, total = item.total.ToString(), orderDate = item.orderDate.ToString(), delivayStatus = item.delivayStatus });
                }
                return Json(new { data = lstorder }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]

        public ActionResult ViewOrder(int id)
        {
            using (WebDBEntities db = new WebDBEntities())
            {

                List<OrderDetailViewModel> lstod = new List<OrderDetailViewModel>();
                var empList = db.tbl_OrderDetail.Where(x => x.orderID == id).ToList();
                foreach (tbl_OrderDetail item in empList)
                {
                    lstod.Add(new OrderDetailViewModel() { productID = Convert.ToInt32(item.productID), title = item.tbl_Product.title, quantity = Convert.ToInt32(item.quantity), unitPrice = Convert.ToDecimal(item.unitPrice) });
                }
                Session.Add("itemlist", lstod);
                Session.Add("orderID", id);
                return View(lstod);
            }
        }
        [HttpPost, ActionName("ViewOrder")]
        public ActionResult ViewOrder_Post(int id)
        {
            using (WebDBEntities db = new WebDBEntities())
            {

                tbl_Order sm = db.tbl_Order.Where(x => x.orderID == id).FirstOrDefault();
                sm.delivayStatus = "Confirmed";
                db.SaveChanges();
                return RedirectToAction("ManageOrder", "Order");
            }

        }
        WebDBEntities db = new WebDBEntities();
        public ActionResult PrintBill()
        {
            List<OrderDetailViewModel> lst = null;
            if (Session["itemlist"] != null)
            {
                lst = (List<OrderDetailViewModel>)Session["itemlist"];
                ViewBag.orderlst = lst;
                if (Session["orderid"] != null)
                {
                    int oid = Convert.ToInt32(Session["orderid"].ToString());
                    BillViewModel blv = new BillViewModel();
                    tbl_Order tbo = db.tbl_Order.Where(o => o.orderID == oid).FirstOrDefault();
                    ViewBag.Fullname = tbo.firstname + " " + tbo.lastname;
                    ViewBag.Phone = tbo.phone;
                    ViewBag.Address = tbo.address;
                    ViewBag.OrderDate = tbo.orderID;

                }

            }
            return View(lst);


        }


        [HttpGet]

        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (WebDBEntities db = new WebDBEntities())
                {
                    // ViewBag.Menus = db.tblMenus.ToList();
                    return View(new tbl_Category());
                }
            }
            else
            {
                using (WebDBEntities db = new WebDBEntities())
                {
                    // ViewBag.Menus = db.tblMenus.ToList();
                    return View(db.tbl_Category.Where(x => x.categoryID == id).FirstOrDefault());
                }
            }
        }

        [HttpPost]

        public ActionResult AddOrEdit(tbl_Category sm)
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                if (sm.categoryID == 0)
                {
                    db.tbl_Category.Add(sm);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(sm).State = EntityState.Modified;
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
                tbl_Order sm = db.tbl_Order.Where(x => x.orderID == id).FirstOrDefault();
                db.tbl_Order.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}