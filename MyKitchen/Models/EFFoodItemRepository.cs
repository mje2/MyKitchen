using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyKitchen.Data;

namespace MyKitchen.Models
{
    public class EFFoodItemRepository:IFoodItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public EFFoodItemRepository(ApplicationDbContext ctx,ILogger<EFFoodItemRepository> logger)
        {
            _context = ctx;
            _logger = logger;
        }

        //Returns all Food Items in the DB - should only be used when we don't care about this user who created the item
        public IQueryable<FoodItem> FoodItems => _context.FoodItems.Include(x => x.FoodGroup);
        
       //Add's a new FoodItem to the globa database - these food items are not yet associated with a user. 
        public Task<int> Add(FoodItem foodItem)
        {
            _context.FoodItems.Add(foodItem);
            _logger.LogInformation($"added food item {foodItem.FoodItemName}");
            return _context.SaveChangesAsync();
        }

        //Adds an association between a food item and a user.
        public async Task<int> AddFoodForUser(ApplicationUser user, FoodItem foodItem)
        {
            var userFoodItem = new MyKitchen.Data.UserFoodItem();
            userFoodItem.FoodItemID = foodItem.FoodItemID;
            userFoodItem.AppUser = user;

            _context.UserFoodItems.Add(userFoodItem);
            return await _context.SaveChangesAsync();
        }

        public Task<FoodItem> Find(int id)
        {
            #region Example Code - Logging 
            // _logger.LogTrace("TRACE TEST - search for item");
            // _logger.LogDebug("DEUB TEST - search for item");
            // _logger.LogInformation("Information TEST - search for item");
            // _logger.LogWarning("WARNING TEST - search for item");
            // _logger.LogError("ERROR TEST - search for item");
            // _logger.LogCritical("CRITICAL TEST - search for item");
            #endregion
            return _context.FoodItems.FindAsync(id).AsTask();
        }

        protected Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        protected void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(FoodItem foodItem)
        {
            _context.FoodItems.Update(foodItem);
            _context.SaveChanges();
        }

        public void Remove(FoodItem foodItem)
        {
            //Because FoodItems is set to OnDelete = Cascade, deleting UserFoodItems should not be necessary, but we will leave it here in case
            //we decide not to use the cascade.

            var userFoodItems = _context.UserFoodItems.Where(x => x.FoodItemID == foodItem.FoodItemID).ToList();
            _context.UserFoodItems.RemoveRange(userFoodItems);

            _context.FoodItems.Remove(foodItem);
            _context.SaveChanges();
            
        }

        public FoodItem GetRandomItem()
        {
            return _context.FoodItems.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }

        public IEnumerable<FoodItem> GetFoodItems()
        {
            return FoodItems.AsEnumerable();
        }
        public IQueryable<FoodItem> GetFoodItemsForUser(ApplicationUser user)
        {
            
            // return _context.FoodItems.Include(x => x.FoodGroup).Where(_context.UserFoodItems.Where);

            var cresult = (from userFoodItmes in _context.UserFoodItems
                              join foodItems in _context.FoodItems.Include(x => x.FoodGroup)
                              on userFoodItmes.FoodItemID equals foodItems.FoodItemID
                              where userFoodItmes.AppUser.Id == user.Id
                              select foodItems).AsQueryable();

            return cresult;

            // var userFoodItems = _context.UserFoodItems.Where(x => x.AppUser.Id == user.Id).Select(x => x.FoodItems
            // .Include(x => x.FoodGroups)).ToLIst();
            // return userFoodItems;
        }
        IQueryable<FoodItem> IFoodItemRepository.GetFoodItems()
        {
            return FoodItems.AsQueryable();
        }
    }
}
