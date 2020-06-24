using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyKitchen.Data.Calendar
{
    public class Events
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
      
        [Required]
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string ThemeColor { get; set; }

        [Required]
        public bool IsFullDay { get; set; }

        public ApplicationUser AppUser {get; set;}

        [ForeignKey("FoodItems")]
        public int FoodItemID { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual FoodItem FoodItems { get; set; }
    }
}
