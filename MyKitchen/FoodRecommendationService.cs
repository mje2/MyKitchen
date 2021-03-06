﻿using System;
using System.Linq;
using MyKitchen.BL;
using MyKitchen.Data;

    namespace MyKitchen
{
    public class FoodRecommendationService:IFoodReccomendationService
    {
        public UserInfo CurrentUser { get; }

        private ApplicationDbContext ctx;
        public FoodRecommendationService(ApplicationDbContext pctx, UserInfo user)
        {
            CurrentUser = user;
            ctx = pctx;
        }

        public string ServiceName { get; set; }
        public string GetNextRecommendation()
        {
            var items = ctx.vwsUserMealsAndFoodItems.Where(x => CurrentUser.User.Id == x.AppUserId).AsQueryable();
            var rec = SelectRandItem(items);

            if(rec == null) return "You don't have any food items added yet. Click Here to Add Food";
            return rec.ItemName;
        }

        private T SelectRandItem<T>(IQueryable<T> queryable)
        {
            return queryable.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }
    }
}