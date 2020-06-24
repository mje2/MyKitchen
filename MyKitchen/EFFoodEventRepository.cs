using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyKitchen;
using MyKitchen.Data;
using MyKitchen.Data.Calendar;

namespace MyKitchen.Models
{

    public class EFFoodEventRepository:IFoodEventRepository
    {

       private ApplicationDbContext Context;

        public EFFoodEventRepository(ApplicationDbContext ctx)
        {
            Context = ctx;
        }

        public void AddFoodEvent(ApplicationUser user, Events foodItem)
        {
            Context.Events.Add(foodItem);
            SaveChanges();

        }

        public Events Find(int id)
        {
            var events = Context.Events.Include(x => x.FoodItems).Where(x => x.EventID == id).FirstOrDefault();
            return events;
        }

        public IEnumerable<Events> GetFoodEvents(ApplicationUser user)
        {
            //TODO need to implement paging in our repositoryn classes 
            return Context.Events.AsEnumerable();
        }

        public void Remove(Events foodItem)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(int month, ApplicationUser user1)
        {
            List<Events> monthEvents = Context.Events.Where(x => x.Start.Month == month).ToList();
            Context.Events.RemoveRange(monthEvents);
            SaveChanges();
        }

       public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void Update(Events event1)
        {
            var updateEvent = Context.Events.FirstOrDefault(x => x.EventID == event1.EventID);
            
           //does this really work? 
            updateEvent.FoodItems = event1.FoodItems;

            updateEvent.Start = event1.Start;
            SaveChanges();
        }
    }




    
}
