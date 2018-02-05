using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public OrderDetailsCart detailCart { get; set; }

        public void OnGet()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new OrderHeader()
            };
            detailCart.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }
            foreach (var list in detailCart.listCart)
            {
                list.MenuItem = _db.MenuItem.FirstOrDefault(m => m.Id == list.MenuItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (list.MenuItem.Price * list.Count);
                if (list.MenuItem.Descrption.Length > 100)
                {
                    list.MenuItem.Descrption = list.MenuItem.Descrption.Substring(0, 99) + "...";
                }
            }
            detailCart.OrderHeader.PickUpTime = DateTime.Now;

        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _db.ShoppingCart.Where(c => c.Id == cartId).FirstOrDefault();
            cart.Count += 1;
            _db.SaveChanges();
            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _db.ShoppingCart.Where(c => c.Id == cartId).FirstOrDefault();
            if (cart.Count == 1)
            {
                _db.ShoppingCart.Remove(cart);
                _db.SaveChanges();
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32("CartCount", _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count);
            }
            else
            {
                cart.Count -= 1;
                _db.SaveChanges();
            }
            
            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPost()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            detailCart.listCart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value).ToList();

            OrderHeader orderHeader = detailCart.OrderHeader;
            detailCart.OrderHeader.OrderDate = DateTime.Now;
            detailCart.OrderHeader.UserId = claim.Value;
            detailCart.OrderHeader.Status = SD.StatusSubmitted;
            _db.OrderHeader.Add(orderHeader);
            _db.SaveChanges();

            foreach (var item in detailCart.listCart)
            {
                item.MenuItem = _db.MenuItem.FirstOrDefault(m => m.Id == item.MenuItemId);
                OrderDetail orderDetails = new OrderDetail
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = orderHeader.Id,
                    Name = item.MenuItem.Name,
                    Description = item.MenuItem.Descrption,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };
                _db.OrderDetail.Add(orderDetails);
            }
            _db.ShoppingCart.RemoveRange(detailCart.listCart);
            HttpContext.Session.SetInt32("CartCount", 0);
            _db.SaveChanges();
            return RedirectToPage("../Order/OrderConfirmation", new  { id=orderHeader.Id });
        }
    }
}