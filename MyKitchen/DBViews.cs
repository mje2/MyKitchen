using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyKitchen.Data;

namespace MyKitchen.Models
{
    public class DBViews{

        private ApplicationDbContext Context;

        public  DBViews(ApplicationDbContext context)
        {
            Context = context;

        }

        public IEnumerable<vwsMealsAndFoodItems> VwsMealsAndFoodItems(){

            return Context.vwsMealsAndFoodItems.AsEnumerable();
        }

        public IEnumerable<vwsMealsAndFoodItems> VwsMealsAndFoodItems(ApplicationUser user)
        {
            return Context.vwsMealsAndFoodItems.Where(x => x.AppUserId == user.Id).AsEnumerable();

        }




    }




    
}
