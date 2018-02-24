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
    public class ManageOrderModel : PageModel
    {
        private ApplicationDbContext _db;

        public ManageOrderModel(ApplicationDbContext db)
        {
            _db = db;
            OrderDetailsViewModel = new List<ViewModel.OrderDetailsViewModel>();
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        public void OnGet()
        {
            List<OrderHeader> OrderHeaderList = _db.OrderHeader.Where(u => u.Status != SD.StatusCompleted && u.Status != SD.StatusReady && u.Status != SD.StatusCancelled).OrderByDescending(u => u.PickUpTime).ToList();

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

        public IActionResult OnPostOrderReady(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.Find(orderId);
            orderHeader.Status = SD.StatusReady;
            _db.SaveChanges();
            return RedirectToPage("/Order/ManageOrder");
        }

        public IActionResult OnPostOrderCancel(int orderId)
        {
            OrderHeader orderHeader = _db.OrderHeader.Find(orderId);
            orderHeader.Status = SD.StatusCancelled;
            _db.SaveChanges();
            return RedirectToPage("/Order/ManageOrder");
        }
    }
}