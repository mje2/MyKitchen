﻿using System.Collections.Generic;
using MyKitchen.Data;

namespace MyKitchen.Models.Meals
{
    public class MealBuilderCreateViewModel
    {
        public Meal Meal { get; set; }
        public IEnumerable<FoodItem> FoodItems { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}