using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasteRestaurant.Data;

namespace TasteRestaurant.ViewModel
{
    public class MenuItemViewModel
    {
        public MenuItem MenuItem { get; set; }
        public IEnumerable<CategoryType> CategoryType { get; set; }
        public IEnumerable<FoodType> FoodType { get; set; }
    }
}
