using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication12.Models
{
    public class ShoppingCart
    {
        WebDBEntities storeDB = new WebDBEntities();
        string shoppingCartID { get; set; }
        public const string CartSessionKey = "cartID";
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.shoppingCartID = cart.GetCartId(context);
            return cart;
        }
     
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(tbl_Product product)
        {
           
            var cartItem = storeDB.tbl_Cart.SingleOrDefault(
                c => c.cartID == shoppingCartID
                && c.productID == product.productID);

            if (cartItem == null)
            {
              
                cartItem = new tbl_Cart
                {
                    productID = product.productID,
                    cartID = shoppingCartID,
                    count = 1,
                    dateCreated = DateTime.Now
                };
                storeDB.tbl_Cart.Add(cartItem);
            }
            else
            {
                cartItem.count++;
            }
            // Save changes 
            storeDB.SaveChanges();
        }

        public int UpdateCartCount(int id, int cartCount)
        {
            // Get the cart 
            var cartItem = storeDB.tbl_Cart.Single(
                cart => cart.cartID == shoppingCartID
                && cart.recordID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartCount > 0)
                {
                    cartItem.count = cartCount;
                    itemCount = Convert.ToInt32(cartItem.count);
                }
                else
                {
                    storeDB.tbl_Cart.Remove(cartItem);
                }
                // Save changes 
                storeDB.SaveChanges();
            }
            return itemCount;
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart 
            var cartItem = storeDB.tbl_Cart.Single(
                cart => cart.cartID == shoppingCartID
                && cart.recordID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
               
                storeDB.tbl_Cart.Remove(cartItem);
              
                storeDB.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = storeDB.tbl_Cart.Where(
                cart => cart.cartID == shoppingCartID);

            foreach (var cartItem in cartItems)
            {
                storeDB.tbl_Cart.Remove(cartItem);
            }
            // Save changes 
            storeDB.SaveChanges();
        }

        public List<tbl_Cart> GetCartItems()
        {
            return storeDB.tbl_Cart.Where(
                cart => cart.cartID == shoppingCartID).ToList();
      
        }

        public int GetCount()
        {
          
            int? count = (from cartItems in storeDB.tbl_Cart
                          where cartItems.cartID == shoppingCartID
                          select (int?)cartItems.count).Sum();
         
            return count ?? 0;
        }

        public decimal GetTotal()
        {
           
            decimal? total = (from cartItems in storeDB.tbl_Cart
                              where cartItems.cartID == shoppingCartID
                              select (int?)cartItems.count * cartItems.tbl_Product.sellingPrice).Sum();
            return total ?? decimal.Zero;
        }

        public int CreateOrder(tbl_Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();
         
            foreach (var item in cartItems)
            {
                var orderDetail = new tbl_OrderDetail
                {
                    productID = item.productID,
                    orderID = order.orderID,
                    unitPrice = item.tbl_Product.sellingPrice,
                    quantity = item.count
                };
               
                orderTotal += Convert.ToDecimal(item.count * (item.tbl_Product.sellingPrice));
                storeDB.tbl_OrderDetail.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count 
            order.total = orderTotal;
            storeDB.SaveChanges();          
            EmptyCart();
            return order.orderID;
        }

        //We're using HttpContextBase to allow access to cookies. 
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                  
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.tbl_Cart.Where(
                c => c.cartID == shoppingCartID);

            foreach (tbl_Cart item in shoppingCart)
            {
                item.cartID = userName;
            }
            storeDB.SaveChanges();
        }
    }
}