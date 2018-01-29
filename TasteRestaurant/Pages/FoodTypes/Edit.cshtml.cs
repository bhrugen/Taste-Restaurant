using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;

namespace TasteRestaurant.Pages.FoodTypes
{
    public class EditModel : PageModel
    {
        private readonly TasteRestaurant.Data.ApplicationDbContext _context;

        public EditModel(TasteRestaurant.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FoodType FoodType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FoodType = await _context.FoodType.SingleOrDefaultAsync(m => m.Id == id);

            if (FoodType == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FoodType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodTypeExists(FoodType.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FoodTypeExists(int id)
        {
            return _context.FoodType.Any(e => e.Id == id);
        }
    }
}
