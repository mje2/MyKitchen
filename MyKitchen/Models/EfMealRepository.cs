using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyKitchen.Controllers;
using MyKitchen.Data;

namespace MyKitchen.Models
{
    public class EfMealRepository : IMealRepository
    {
        private ApplicationDbContext context;

        public EfMealRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public Task<int> Add(Meal meal)
        {
            context.Meals.Add(meal);

            if (meal.MealFoodItems != null)
            {
                //add meal items
                foreach (var foodItem in meal.MealFoodItems)
                {
                    //add items to meal
                }
            }

            return context.SaveChangesAsync();
        }

        public Task<Meal> FindAsync(int id)
        {

            var meal   = context.Meals.Where(x => x.MealID == id).Include(x => x.MealFoodItems).ThenInclude(x => x.FoodItems).FirstOrDefaultAsync();
            return meal;
        }

        public Meal Find(int id)
        {

            IIncludableQueryable<Meal, FoodItem> wackyEntity = context.Meals.Include(x => x.MealFoodItems).ThenInclude(x => x.FoodItems);

            return wackyEntity.FirstOrDefault(x => x.MealID == id);

        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public void Update(Meal foodItem)
        {
            throw new NotImplementedException();
        }

        public void Remove(Meal foodItem)
        {
            context.Remove(foodItem);
        }

        public Meal GetRandomItem()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Meal> GetMeals()
        {
            return context.Meals.Include(x => x.MealFoodItems).ThenInclude(x => x.FoodItems).AsEnumerable();
        }

        private IEnumerable<Meal> GetEnumerable()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Meals.Count();
        }

        public Meal GetMealById(int mealId)
        {
            var meal = context.Meals.Where(x => x.MealID == mealId).Include(x => x.MealFoodItems).FirstOrDefault();
            return meal;

        }

        public IQueryable<Meal> GetMealsForUser(ApplicationUser user)
        {
            var _context = this.context;

            //figure out how to include a list of all food items in each meal as part of this query and look how it is performing the query.
            var cresult = (from meals in _context.Meals.Include(x => x.MealFoodItems).ThenInclude(x => x.FoodItems)
                           where meals.AppUser.Id == user.Id select meals).AsQueryable();

            return cresult;


        }
        public int CountForUser(ApplicationUser user)
        {
            var _context = this.context;
            var cresult = (from meals in _context.Meals.Include(x => x.MealFoodItems).ThenInclude(x => x.FoodItems)
                           where meals.AppUser.Id == user.Id select meals).AsQueryable().Count();

            return cresult;
        }
    }
}
