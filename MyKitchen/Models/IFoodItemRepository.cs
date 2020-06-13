using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyKitchen.Data;


namespace MyKitchen.Models
{
    public interface IFoodItemRepository
    {

        IQueryable<FoodItem> GetFoodItems();
        IQueryable<FoodItem> GetFoodItemsForUser(ApplicationUser user);
        Task<int> Add(FoodItem foodItem);
        //Adds item for a specific user
        Task<int> AddFoodForUser(ApplicationUser user, FoodItem foodItem);
        Task<FoodItem> Find(int id);
        Task SaveChangesAsync();
        void Update(FoodItem foodItem);
        void Remove(FoodItem foodItem);
        FoodItem  GetRandomItem();
    }


    
}
