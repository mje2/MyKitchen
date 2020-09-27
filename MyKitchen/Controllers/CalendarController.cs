using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyKitchen.BL;
using MyKitchen.Data;
using MyKitchen.Data.Calendar;
using MyKitchen.Models;

namespace MyKitchen.Controllers
{
    public class CalendarController : Controller
    {

        //Dependencies
        private IFoodEventRepository FoodEventRepo {get; set;}
        private MyKitchen.Models.DBViews DBViews {get; set;}
        private readonly ILogger _logger;
        private UserInfo CurrentUser {get; set;}

        //Properties
        public int PageSize = 10;


        public CalendarController(IFoodEventRepository foodEventRepo,
                                  ILogger<CalendarController> logger, 
                                  DBViews dBViews,
                                  UserInfo userInfo)
        {
            DBViews = dBViews;
            FoodEventRepo = foodEventRepo;
            CurrentUser = userInfo;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            var events = FoodEventRepo.GetFoodEvents(CurrentUser.User).ToList();
            return new JsonResult(events);
        }

        public JsonResult GetEventsFeed()
        {
            var events = FoodEventRepo.GetFoodEvents(CurrentUser.User).ToList().Select(FullCalendarEvent.FromEvent);
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
            //should only show items for the current user
            var items = this.DBViews.VwsMealsAndFoodItems(this.CurrentUser.User).ToList();
            return new JsonResult(items);
        }



        [HttpPost]
        public JsonResult SaveNewEvent([FromBody] Events event1)
        {
            FoodEventRepo.AddFoodEvent(CurrentUser.User,event1);
            return new JsonResult(true);
        }

        [HttpPost]
        public JsonResult UpdateEvent([FromBody]Events event1)
        {
            if(event1.EventID < 1) {  return new JsonResult(false);}
            FoodEventRepo.Update(event1);

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

            FoodEventRepo.RemoveRange(smonth,CurrentUser.User);




            return new JsonResult(true);
        }
    }
}