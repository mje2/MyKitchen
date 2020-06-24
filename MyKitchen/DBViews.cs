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

        public DbSet<vwsMealsAndFoodItems> VwsMealsAndFoodItems(){

            return Context.vwsMealsAndFoodItems;
        }

    }




    
}
