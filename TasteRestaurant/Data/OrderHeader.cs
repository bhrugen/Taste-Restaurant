using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Data
{
    public class OrderHeader
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public double OrderTotal { get; set; }
        [Required]
        public DateTime PickUpTime { get; set; }

        public string Status { get; set; }
        public string Comments { get; set; }
    }
}
