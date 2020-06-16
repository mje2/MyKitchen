using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyKitchen.Data;
using MyKitchen.Data.Calendar;

namespace MyKitchen.Controllers
{
    public class CalendarController : Controller
    {
        //allow food items or meals to be added to the calendar


        private ApplicationDbContext ctx { get; set; }

        public int PageSize = 10;

        public CalendarController(ApplicationDbContext context)
        {
            ctx = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            var events = ctx.Events.ToList();
            return new JsonResult(events);
        }

        public JsonResult GetEventsFeed()
        {
            var events = ctx.Events.ToList().Select(FullCalendarEvent.FromEvent);
            return new JsonResult(events);
        }


        public class FullCalendarEvent
        {
            //mirroring fullcalendar object properties
            public string  title { get; set; }
            public string description { get; set; }
            public DateTime start { get; set; }
            public DateTime? end { get; set; }
            public string color { get; set; }
            public bool allDay { get; set; }
            public int eventID { get; set; }

            public static FullCalendarEvent FromEvent(Events evt)
            {
                var fullCalendarEvent = new FullCalendarEvent
                {
                    title = evt.Subject,
                    description = evt.Description,
                    start = evt.Start,
                    end = evt?.End,
                    color = evt.ThemeColor,
                    eventID = evt.EventID,
                    allDay = evt.IsFullDay

                };

                return fullCalendarEvent;
            }

        }

        public JsonResult GetAvailableItems()
        {
            var items = ctx.vwsMealsAndFoodItems.ToList();
            return new JsonResult(items);
        }



        [HttpPost]
        public JsonResult SaveNewEvent([FromBody] Events event1)
        {

            //Save new Event to DB
            ctx.Events.Add(event1);
            ctx.SaveChanges();

            return new JsonResult(true);


        }

        [HttpPost]
        public JsonResult UpdateEvent([FromBody]Events event1)
        {
            if(event1.EventID < 1) {  return new JsonResult(false);}

            var updateEvent = ctx.Events.FirstOrDefault(x => x.EventID == event1.EventID);
            updateEvent.Start = event1.Start;
            ctx.SaveChanges();

            return new JsonResult(true);
        }


        public class CalendarCommand
        {
            public int Month { get; set; }
        }

        [HttpPost]
        public JsonResult ClearMonth([FromBody]  CalendarCommand cmd)
        {
            var smonth = cmd.Month + 1;

            List<Events> monthEvents = ctx.Events.Where(x => x.Start.Month == smonth).ToList();
            ctx.Events.RemoveRange(monthEvents);
            ctx.SaveChanges();

            return new JsonResult(true);
        }
    }
}