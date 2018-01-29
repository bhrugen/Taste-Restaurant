using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Data
{
    public class CategoryType
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name="Display Order")]
        public int DisplayOrder { get; set; }
    }
}
