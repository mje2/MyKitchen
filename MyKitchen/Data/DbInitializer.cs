using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyKitchen.Models;

namespace MyKitchen.Data
{
    public class DbInitializer
    {
        private ApplicationDbContext Context {get; set;}
        private UserManager<ApplicationUser> UserManager {get; set;}
        private IFoodItemRepository FoodItemRepository {get; set;}

        private DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> usermanager,IFoodItemRepository foodItemRepository){

           this.Context = context;
           this.UserManager  =  usermanager;
           this.FoodItemRepository = foodItemRepository;
        }

        private void InitializeDatabase(){

            Context.Database.Migrate();
            InitializeFoodItems(Context,UserManager,FoodItemRepository);
            InitializeFoodGroups(Context);
        }

        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> usermanager,IFoodItemRepository foodItemRepository)
        {
            var dbInitializer = new DbInitializer(context, usermanager,foodItemRepository);
            dbInitializer.InitializeDatabase();
        }

        private void InitializeFoodGroups(ApplicationDbContext context)
        {
            if (context.FoodGroups.Any())
            {
                return;//DB has been seeded.
            }

            var seedFoodGroups = new FoodGroup[]
            {
                new FoodGroup(){Name = "Vegetables"},
                new FoodGroup(){Name = "Fruits"},
                new FoodGroup(){Name = "Grains"},
                new FoodGroup(){Name = "Protein"},
                new FoodGroup(){Name = "Dairy"},
                new FoodGroup(){Name = "Oils"},
                new FoodGroup(){Name = "Other Calories"}
            };

            foreach (var item in seedFoodGroups)
            {
                context.FoodGroups.Add(item);
            }

            context.SaveChanges();
        }

        private static void InitializeFoodItems(ApplicationDbContext context,UserManager<ApplicationUser> userManager,IFoodItemRepository foodItemRepo)
        {
            //TODO do everything with the repository here.
            //add these food items to our test user, and to any new users who are registered.
            var user = userManager.FindByEmailAsync("matteskolin@gmail.com").GetAwaiter().GetResult();

            if (context.FoodItems.Any())
            {
                return; // DB has been seeded
            }


            var seedFoodItems = new FoodItem[]
            {
                new FoodItem() {Cost = 0.00M, FoodDescription = "", FoodItemName = "Mixed Greens"},
                new FoodItem() {Cost = 0.00M, FoodDescription = "", FoodItemName = "Scrambled Egg"},
                new FoodItem() {Cost = 0.00M, FoodDescription = "", FoodItemName = "Fried Egg"},
                new FoodItem() {Cost = 0.00M, FoodDescription = "", FoodItemName = "Breakfast Sausage"}
            };

            //add to test user
            foreach(var item in seedFoodItems)
            {
                foodItemRepo.Add(item).GetAwaiter().GetResult();
                foodItemRepo.AddFoodForUser(user,item).GetAwaiter().GetResult();;
            }
        }
    }
}
