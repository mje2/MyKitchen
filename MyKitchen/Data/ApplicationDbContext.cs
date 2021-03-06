﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyKitchen.Controllers;
using MyKitchen.Data.Calendar;

namespace MyKitchen.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> , IMyKitchenDataContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public ApplicationDbContext(){}


        public DbSet<Meal> Meals { get; set; }

        public DbSet<LastCookedMeal> LastCookedMeal { get; set; }

        public DbSet<MealFoodItems> MealFoodItems { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }

        public DbSet<UserFoodItem> UserFoodItems {get; set;}


        public DbSet<Events> Events { get; set; }

        public DbSet<vwsMealsAndFoodItems> vwsMealsAndFoodItems { get; set; }

        public DbSet<vwsMealItems> vwsMealItems {get; set;}

        public virtual DbSet<vwsUserMealsAndFoodItem> vwsUserMealsAndFoodItems { get; set;}

        public DbSet<FoodGroup> FoodGroups { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MealFoodItems>(x => { x.HasIndex(y => new {y.FoodItemId, y.MealId}).IsUnique(); });

            builder.Entity<FoodItem>().HasOne(x => x.FoodGroup).WithMany(y => y.FoodItem);

            builder.Entity<vwsMealsAndFoodItems>().HasNoKey();

            builder.Entity<vwsMealItems>().HasNoKey();

            builder.Entity<vwsUserMealsAndFoodItem>().HasNoKey();

        }
    }

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FavoriteFood { get; set; }
    }

}
