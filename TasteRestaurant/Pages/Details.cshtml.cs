using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;

namespace TasteRestaurant.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ShoppingCart CartObj { get; set; }

        public void OnGet(int id)
        {
            var MenuItemFromDb=_db.MenuItem.Include(m=>m.CategoryType).Include(m=>m.FoodType).FirstOrDefault();


            CartObj = new ShoppingCart()
            {
                MenuItemId=MenuItemFromDb.Id,
                MenuItem=MenuItemFromDb
            };

        }
    }
}