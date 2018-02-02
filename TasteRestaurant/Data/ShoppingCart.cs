using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Data
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int MenuItemId { get; set; }

        [NotMapped]
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem{ get; set; }

        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
