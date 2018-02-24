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

        public void OnGet(string option = null, string search = null)
        {
            if (search != null)
            {
                var user = new ApplicationUser();
                List<OrderHeader> OrderHeaderList = new List<OrderHeader>();
                if (option == "order")
                {
                    OrderHeaderList = _db.OrderHeader.Where(o => o.Id == Convert.ToInt32(search)).ToList();
                }
                else
                {
                    if (option == "email")
                    {
                        user = _db.Users.Where(u => u.Email.ToLower().Contains(search.ToLower())).FirstOrDefault();
                    }
                    else
                    {
                        if (option == "phone")
                        {
                            user = _db.Users.Where(u => u.PhoneNumber.ToLower().Contains(search.ToLower())).FirstOrDefault();
                        }
                        else
                        {
                            if (option == "name")
                            {
                                user = _db.Users.Where(u => u.FirstName.ToLower().Contains(search.ToLower()) || u.LastName.ToLower().Contains(search.ToLower())).FirstOrDefault();
                            }
                        }
                    }
                }
                if (user != null || OrderHeaderList.Count > 0)
                {
                    if (OrderHeaderList.Count == 0)
                    {
                        OrderHeaderList = _db.OrderHeader.Where(o => o.UserId == user.Id).OrderByDescending(o => o.PickUpTime).ToList();
                    }
                    foreach (OrderHeader item in OrderHeaderList)
                    {
                        OrderDetailsViewModel individual = new ViewModel.OrderDetailsViewModel();
                        individual.OrderDetail = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList();
                        individual.OrderHeader = item;

                        OrderDetailsViewModel.Add(individual);
                    }

                }
            }
            else
            {
                //If there is no search criteria
                List<OrderHeader> OrderHeaderList = _db.OrderHeader.Where(u => u.Status == SD.StatusReady).OrderByDescending(u => u.PickUpTime).ToList();

                foreach (OrderHeader item in OrderHeaderList)
                {
                    OrderDetailsViewModel individual = new ViewModel.OrderDetailsViewModel();
                    individual.OrderDetail = _db.OrderDetail.Where(o => o.OrderId == item.Id).ToList();
                    individual.OrderHeader = item;

                    OrderDetailsViewModel.Add(individual);
                }
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