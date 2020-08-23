using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class UserController : Controller
    {
        WebDBEntities _db = new WebDBEntities();
        // GET: User
        public ActionResult ManageUser()
        {
            return View();
        }

        public JsonResult GetData()
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<UserViewModel> lst = new List<UserViewModel>();
                var userList = db.tbl_User.ToList();
                foreach (var item in userList)
                {
                    lst.Add(new UserViewModel() { userID = item.userID, username = item.username, password = item.password, fullname = item.fullname,  email = item.email });
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
                    ViewBag.Action = "New User";
                    return View(new UserViewModel());
                }
            }
            else
            {
                using (WebDBEntities db = new WebDBEntities())
                {
                    UserViewModel sub = new UserViewModel();
                    var menu = db.tbl_User.Where(x => x.userID == id).FirstOrDefault();
                    sub.userID = menu.userID;
                    sub.username = menu.username;
                    sub.password = menu.password;
                    sub.fullname = menu.fullname;                    
                    sub.email = menu.email;
                    ViewBag.Action = "Edit User";
                    return View(sub);
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(UserViewModel sm)
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                if (sm.userID == 0)
                {
                    tbl_User tb = new tbl_User();
                    tb.username = sm.username;
                    tb.password = sm.password;
                    tb.fullname = sm.fullname;                    
                    tb.email = sm.email;
                    db.tbl_User.Add(tb);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tbl_User tbm = db.tbl_User.Where(m => m.userID == sm.userID).FirstOrDefault();
                    tbm.username = sm.username;
                    tbm.password = sm.password;
                    tbm.fullname = sm.fullname;                    
                    tbm.email = sm.email;
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
                tbl_User sm = db.tbl_User.Where(x => x.userID == id).FirstOrDefault();
                db.tbl_User.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}