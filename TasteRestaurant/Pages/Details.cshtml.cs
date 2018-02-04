using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;

namespace TasteRestaurant.Pages
{

    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [TempData]
        public string StatusMessage { get; set; }

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ShoppingCart CartObj { get; set; }

        public void OnGet(int id)
        {
            var MenuItemFromDb=_db.MenuItem.Include(m=>m.CategoryType).Include(m=>m.FoodType)
                                    .Where(x=>x.Id==id).FirstOrDefault();


            CartObj = new ShoppingCart()
            {
                MenuItemId=MenuItemFromDb.Id,
                MenuItem=MenuItemFromDb
            };
        }


        public IActionResult OnPost()
        {

            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                CartObj.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObj.ApplicationUserId
                                            && c.MenuItemId == CartObj.MenuItemId).FirstOrDefault();

                if (cartFromDb == null)
                {
                    _db.ShoppingCart.Add(CartObj);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + CartObj.Count;
                }
                _db.SaveChanges();

                //Add Session and increment count
                var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObj.ApplicationUserId).ToList().Count();
                HttpContext.Session.SetInt32("CartCount", count);
                StatusMessage = "Item Added to Cart";
                return RedirectToPage("Index");
            }
            else
            {
                var MenuItemFromDb = _db.MenuItem.Include(m => m.CategoryType).Include(m => m.FoodType).FirstOrDefault();


                CartObj = new ShoppingCart()
                {
                    MenuItemId = MenuItemFromDb.Id,
                    MenuItem = MenuItemFromDb
                };

                return Page();
            }
        }
    }
}