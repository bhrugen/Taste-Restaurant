using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class OrderPickupDetailsModel : PageModel
    {
        private ApplicationDbContext _db;

        public OrderPickupDetailsModel(ApplicationDbContext db)
        {
            _db = db;
            OrderDetailsViewModel = new OrderDetailsViewModel();
        }

        [BindProperty]
        public OrderDetailsViewModel OrderDetailsViewModel { get; set; }

        public void OnGet(int orderId)
        {
            OrderDetailsViewModel.OrderHeader = _db.OrderHeader.Where(o => o.Id == orderId).FirstOrDefault();
            OrderDetailsViewModel.OrderHeader.ApplicationUser = _db.Users.Where(u => u.Id == OrderDetailsViewModel.OrderHeader.UserId).FirstOrDefault();
            OrderDetailsViewModel.OrderDetail = _db.OrderDetail.Where(o => o.OrderId == OrderDetailsViewModel.OrderHeader.Id).ToList();
        }

        public IActionResult OnPost(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.Find(orderId);
            orderHeader.Status = SD.StatusCompleted;
            _db.SaveChanges();
            return RedirectToPage("/Order/ManageOrder");
        }
    }
}