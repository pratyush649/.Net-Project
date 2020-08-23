using WebApplication12.Models;

using WebApplication12.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApplication12.Controllers
{
    public class ShoppingCartController : Controller
    {
        WebDBEntities storeDB = new WebDBEntities();
      
        [Authorize]
        public ActionResult ShoppingCartList()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                cartItem = cart.GetCartItems(),
                cartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        
        [Authorize]
        public ActionResult AddToCart(int id)
        {
         
            var addedItem = storeDB.tbl_Product.Single(item => item.productID == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedItem);           
            return RedirectToAction("ShoppingCartList");
        }

     
        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            ShoppingCartRemoveViewModel results = null;
            try
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
  
                string itemName = storeDB.tbl_Cart
                    .Single(item => item.recordID == id).tbl_Product.title;

                int itemCount = cart.UpdateCartCount(id, cartCount);

                string msg = "The quantity of " + Server.HtmlEncode(itemName) +
                        " has been refreshed in your shopping cart.";
                if (itemCount == 0) msg = Server.HtmlEncode(itemName) +
                        " has been removed from your shopping cart.";
             
                results = new ShoppingCartRemoveViewModel
                {
                    message = msg,
                    cartTotal = cart.GetTotal(),
                    cartCount = cart.GetCount(),
                    itemCount = itemCount,
                    deleteID = id
                };
            }
            catch
            {
                results = new ShoppingCartRemoveViewModel
                {
                    message = "Error occurred or invalid input...",
                    cartTotal = -1,
                    cartCount = -1,
                    itemCount = -1,
                    deleteID = id
                };
            }
            return Json(results);

        }

     
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
          
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string albumName = storeDB.tbl_Cart.Single(item => item.recordID == id).tbl_Product.title;
            int itemCount = cart.RemoveFromCart(id);
            var results = new ShoppingCartRemoveViewModel
            {
                message = Server.HtmlEncode(albumName) +
                    " has been removed from your shopping cart.",
                cartTotal = cart.GetTotal(),
                cartCount = cart.GetCount(),
                itemCount = itemCount,
                deleteID = id
            };
            return Json(results);
        }
    

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
        [Authorize]
        public ActionResult AddressAndPayment()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            OrderViewModel ordv = new OrderViewModel();
            ordv.total = Convert.ToString(cart.GetTotal());
            return View(ordv);


        }

        [HttpPost]
        public ActionResult AddressAndPayment(OrderViewModel ovm)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            tbl_Order tb = new tbl_Order();
            tb.username = Session["username"].ToString();
            tb.firstname = ovm.firstname;
            tb.lastname = ovm.lastname;
            tb.address = ovm.address;
            tb.phone = ovm.phone;
            tb.total = cart.GetTotal();
            tb.orderDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            tb.delivayStatus = "Pending";

            storeDB.tbl_Order.Add(tb);
            storeDB.SaveChanges();


            List<tbl_Cart> lst = cart.GetCartItems();
            foreach (var item in lst)
            {
                tbl_OrderDetail tbord = new tbl_OrderDetail();
                tbord.orderID = tb.orderID;
                tbord.productID = item.productID;
                tbord.quantity = item.count;
                tbord.unitPrice = item.tbl_Product.sellingPrice;
                storeDB.tbl_OrderDetail.Add(tbord);
                storeDB.SaveChanges();
            }


            ViewBag.Message = "Your Ordered Done Successfully, It Takes 2/3 Hours In Our working Days";
            return View();


        }
    }
}