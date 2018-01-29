using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Data
{
    public class MenuItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name{ get; set; }
        [Required]
        public string Descrption { get; set; }
        public string Image { get; set; }
        public string Spicyness { get; set; }
        public enum ESpicy { Mild=0,Moderate=1,High=2}

        [Range(1,int.MaxValue,ErrorMessage ="Price should be greater than ${1}")]
        public double Price { get; set; }

        [Display(Name="Cateogry Type")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryType CategoryType { get; set; }

        [Display(Name="Food Type")]
        public int FoodTypeId { get; set; }

        [ForeignKey("FoodTypeId")]
        public virtual FoodType FoodType { get; set; }

    }
}
