using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyKitchen.Data
{
    //not a single ingredient but a component of a meal
    public class FoodItem
    {

        [Key]
        public int FoodItemID {get; set;}

        [ForeignKey("FoodGroup")]
        [Display(Name="Food Group")]
        public int? FoodGroupID { get; set; }

        [Display(Name="Name")]
        [Required]
        public string FoodItemName { get; set; }

        [Display(Name="Restaurant")]
        public string ResturantName { get; set; }

        [Display(Name="Details")]
        public string FoodDescription { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }

        public FoodGroup FoodGroup { get; set; }


    }


}
