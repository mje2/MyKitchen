using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyKitchen.Models;

namespace MyKitchen.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context,UserManager<ApplicationUser> usermanager, IFoodItemRepository foodItemRepository)
        {




            context.Database.Migrate();
            CreateViews(context);
            InitializeFoodItems(context,usermanager,foodItemRepository);
            InitializeFoodGroups(context);


        }

        private static void CreateViews(ApplicationDbContext context)
        {



        }

        private static void InitializeFoodGroups(ApplicationDbContext context)
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
                new FoodItem() {Cost = 0.00M, FoodDescription = "Romaine Lettuce", FoodItemName = "Romaine Lettuce"},
                new FoodItem() {Cost = 0.00M, FoodDescription = "Baked Beans", FoodItemName = "Canned Baked Beans"},
                new FoodItem() {Cost = 0.00M, FoodDescription = "Cage Free Egg", FoodItemName = "Egg - Scrambled"},
                new FoodItem() {Cost = 0.00M, FoodDescription = "Cage Free Egg", FoodItemName = "Little Sizzlers Sausage"}
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
