using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
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
                HttpContext.Session.SetInt32("CartCount", _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count);
            }
            else
            {
                cart.Count -= 1;
            }
            _db.SaveChanges();
            return RedirectToPage("/Cart/Index");
        }
    }
}