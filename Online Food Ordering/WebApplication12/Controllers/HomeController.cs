using WebApplication12.Models;
using WebApplication12.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace WebApplication12.Controllers
{
    public class HomeController : Controller
    {
        WebDBEntities _db = new WebDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Contact()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Contact(ContactViewModel cvm)
        //{
        //    return View();
        //}
        public ActionResult Services()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(UserViewModel uv)
        {
            tbl_User tbl = _db.tbl_User.Where(u => u.username == uv.username).FirstOrDefault();
            if (tbl != null)
            {
                return Json(new { success = false, message = "User Already Register" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                tbl_User tb = new tbl_User();
                tb.username = uv.username;
                tb.password = uv.password;
                _db.tbl_User.Add(tb);
                _db.SaveChanges();

                tbl_UserRole ud = new tbl_UserRole();
                ud.userID = tb.userID;
                ud.userroleID = 2;
                _db.tbl_UserRole.Add(ud);
                _db.SaveChanges();
                return Json(new { success = true, message = "User Register Successfully" }, JsonRequestBehavior.AllowGet);
            }



        }
        public ActionResult ForgetPassword()
        {
            return View();

            //return RedirectToAction("Index", "Home");
        }
        //[ValidateOnlyIncomingValuesAttribute]
        [HttpPost]

        public ActionResult ForgetPassword(UserViewModel uv)
        {

            if (ModelState.IsValid)
            {
                //https://www.google.com/settings/security/lesssecureapps
                //Make Access for less secure apps=true

                string from = "gfccafe123@gmail.com";
                using (MailMessage mail = new MailMessage(from, uv.username))
                {
                    try
                    {
                        tbl_User tb = _db.tbl_User.Where(u => u.username == uv.username).FirstOrDefault();
                        if (tb != null)
                        {
                            mail.Subject = "Password Recovery";
                            mail.Body = "Your Password is:" + tb.password;

                            mail.IsBodyHtml = false;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential networkCredential = new NetworkCredential(from, "lale649");
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkCredential;
                            smtp.Port = 587;
                            smtp.Send(mail);
                            ViewBag.Message = "Your Password Is Sent to your email";
                        }
                        else
                        {
                            ViewBag.Message = "email Doesnot Exist in Database";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }

                }

            }
            return View();


            //return RedirectToAction("Index", "Home");
        }
        public ActionResult ShoppingCartList()
        {
            return View();
        }
        public ActionResult ProductList(string search, int? page, int id = 0)
        {

            if (id != 0)
            {

                return View(db.tbl_Product.Where(p => p.categoryID == id).ToList().ToPagedList(page ?? 1, 12));
            }
            else
            {
                if (search != "")
                {
                    return View(db.tbl_Product.Where(x => x.description.Contains(search) || x.title.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, 12));
                }
                else
                {
                    return View(db.tbl_Product.ToList().ToPagedList(page ?? 1, 12));
                }

            }

        }




        WebDBEntities db = new WebDBEntities();
        public ActionResult ViewItem(int id)
        {
            return PartialView("_ViewItem", db.tbl_Product.Find(id));
        }
        [HttpGet]
        public ActionResult LoginForm()
        {
            return PartialView("_LoginForm");
        }
        [HttpPost]
        public ActionResult LoginForm(LoginViewModel lg)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return PartialView("_LoginForm");
        }


    }
}