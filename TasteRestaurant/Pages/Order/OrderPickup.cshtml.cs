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
    public class OrderPickupModel : PageModel
    {
        private ApplicationDbContext _db;

        public OrderPickupModel(ApplicationDbContext db)
        {
            _db = db;
            OrderDetailsViewModel = new List<ViewModel.OrderDetailsViewModel>();
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        public void OnGet()
        {
            List<OrderHeader> OrderHeaderList = _db.OrderHeader.Where(u => u.Status == SD.StatusReady).OrderByDescending(u => u.PickUpTime).ToList();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new ViewModel.OrderDetailsViewModel();
                individual.OrderDetail = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList();
                individual.OrderHeader = item;

                OrderDetailsViewModel.Add(individual);
            }
        }

        public IActionResult OnPostOrderPrepare(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.Find(orderId);
            orderHeader.Status = SD.StatusInProcess;
            _db.SaveChanges();
            return RedirectToPage("/Order/ManageOrder");
        }
    }
}