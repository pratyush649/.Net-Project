using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class AccountController : Controller
    {
        WebDBEntities _db = new WebDBEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel l, string ReturnUrl= "")
        {
            using (WebDBEntities db = new WebDBEntities())
            {
                var users = db.tbl_User.Where(a => a.username == l.username && a.password == l.password).FirstOrDefault();
                if (users != null)
                {                   
                    Session.Add("username", users.username);
                    FormsAuthentication.SetAuthCookie(l.username, true);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        Session.Add("userid", users.userID);
                        if (users.tbl_UserRole.Where(r => r.roleID == 1).FirstOrDefault() != null)
                        {
                            return RedirectToAction("DashBoard", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }


                    }
                }
                else
                {

                    ModelState.AddModelError("", "Invalid User");

                }
            }
            return View();

        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgotPasswordViewModel uv)
        {
            using (MailMessage mm = new MailMessage("suneel.em@gmail.com", uv.email))
            {
                tbl_User tb = _db.tbl_User.Where(e => e.email == uv.email).FirstOrDefault();
                if (tb != null)
                {
                    mm.Subject = "Password Recovery";
                    mm.Body = "Your Password is: " + tb.password;

                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();                    
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("lale649@gmail.com", "password");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Password Sent Please Check your email";
                }
                else
                {
                    ViewBag.Message = "Email doesnot exist in our database";
                }
            }
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(UserViewModel uvm)
        {          
            tbl_User tb= new tbl_User();
            tb.username = uvm.username;
            tb.password = uvm.password;
            tb.email = uvm.email;
            tb.fullname = uvm.fullname;
            _db.tbl_User.Add(tb);
            _db.SaveChanges();

            tbl_UserRole utb = new tbl_UserRole();
            int userID = tb.userID;
            int roleID = 2;
            utb.userID = userID;
            utb.roleID = roleID;
            _db.tbl_UserRole.Add(utb);
            _db.SaveChanges();
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]

        public ActionResult ChangePassword(ChangePasswordViewModel ch)
        {
            int userid = Convert.ToInt32(Session["userid"].ToString());

            tbl_User us = _db.tbl_User.Where(u => u.userID == userid && u.password == ch.OldPassword).FirstOrDefault();
            if (us != null)
            {
                us.password = ch.NewPassword;
                _db.SaveChanges();

            }
            else
            {
                ViewBag.Message = "Wrong Old Password";
            }
            return Json(new { success = true, message = "Password Changed Successfully" }, JsonRequestBehavior.AllowGet);
        }


    }
}