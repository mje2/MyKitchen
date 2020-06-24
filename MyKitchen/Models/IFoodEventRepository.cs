using System.Collections.Generic;
using MyKitchen.Data;
using MyKitchen.Data.Calendar;

namespace MyKitchen.Models
{
    public interface IFoodEventRepository
    {
        IEnumerable<Events> GetFoodEvents(ApplicationUser user);
        void AddFoodEvent(ApplicationUser user, Events foodItem);
        Events Find(int id);
        void SaveChanges();
        void Update(Events foodItem);
        void Remove(Events foodItem);

        //custom for this repo
        void RemoveRange(int month, ApplicationUser user1);


    }




    
}
